import urllib
import urllib2
import json


url = 'http://grin-global-test1.agron.iastate.edu:2011/SearchHost/ping'
url = 'http://localhost:2011/SearchHost/ping'

headers = {}

type = 'xml'

if type == 'json':
    headers = {'Content-Type' : 'application/json' }
elif type == 'xml':
    headers = {'Content-Type' : 'text/xml; charset=utf-8' }
    
data = '<ping />'
    
request = urllib2.Request(url, data, headers)
response = urllib2.urlopen(request)
output = response.read()

print output
