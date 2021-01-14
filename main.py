import requests

url = "http://localhost:8008/api/pdfconverter"

files={
  ('file', ('DSCD-11-PL-006.PDF', open('./data/diaries/DSCD-11-PL-002.PDF', 'rb'), 'application/octet-stream'))
}

response = requests.post(url,  files=files)

print(response.text)