# using json (javascript object notation)

import urllib2
import json


url = 'http://grin-global-test1.agron.iastate.edu:2011/SearchHost/rest/Search'
headers = {'Content-Type' : 'application/json' }
data = json.dumps({ 'searchString' : 'b73',
                'ignoreCase' : 'true',
                'autoAndConsecutiveLiterals': 'false',
                'indexNames' : ['crop_trait_observation'],
                'resolverName' : 'Accessions',
                'offset' : 0,
                'limit' : 10000
                })

request = urllib2.Request(url, data, headers)
response = urllib2.urlopen(request)
output = response.read()

print output
