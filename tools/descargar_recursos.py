"""Descarga los dos archivos gratuitos del enlace oficial del tutorial."""
from html.parser import HTMLParser
from http.cookiejar import CookieJar
from pathlib import Path
from urllib.parse import urlencode
from urllib.request import build_opener, HTTPCookieProcessor, Request
import io
import json
import zipfile

ROOT = Path(__file__).resolve().parents[1]
PAGE = 'https://anokolisa.itch.io/sidescroller-pixelart-sprites-asset-pack-forest-16x16'


class PageParser(HTMLParser):
    def __init__(self):
        super().__init__()
        self.token = None
        self.uploads = []

    def handle_starttag(self, tag, attrs):
        a = dict(attrs)
        if tag == 'meta' and a.get('name') == 'csrf_token':
            self.token = a.get('content') or a.get('value')
        if tag == 'a' and 'data-upload_id' in a:
            self.uploads.append(a['data-upload_id'])


def main():
    opener = build_opener(HTTPCookieProcessor(CookieJar()))
    parser = PageParser()
    parser.feed(opener.open(PAGE).read().decode())
    if not parser.token or len(set(parser.uploads)) != 2:
        raise RuntimeError('La pagina cambio: revisar los dos enlaces oficiales antes de descargar.')
    destination = (ROOT / 'My project' / 'Assets' / 'Sprites').resolve()
    destination.mkdir(parents=True, exist_ok=True)
    for upload in dict.fromkeys(parser.uploads):
        request = Request(PAGE + '/file/' + upload,
                          data=urlencode({'csrf_token': parser.token}).encode(),
                          headers={'Referer': PAGE})
        result = json.load(opener.open(request))
        if 'url' not in result or result.get('lightbox'):
            raise RuntimeError('Descarga requiere revision: ' + str(result.get('errors', 'sin enlace directo')))
        data = opener.open(result['url']).read()
        with zipfile.ZipFile(io.BytesIO(data)) as archive:
            for member in archive.infolist():
                target = (destination / member.filename).resolve()
                if not target.is_relative_to(destination):
                    raise ValueError('Ruta no valida en el archivo')
            archive.extractall(destination)
            print('Paquete', upload, ':', len(archive.infolist()), 'entradas,', len(data), 'bytes')


if __name__ == '__main__':
    main()
