import urllib
import urllib2
import json


url = 'http://localhost:2011/SearchHost/restful/Search'
url = 'http://grin-global-test1.agron.iastate.edu:2011/SearchHost/restful/Search'

type = 'json'
type = 'xml'

if type == 'json':
    # using json (javascript object notation)
    headers = {'Content-Type' : 'application/json' }
    data = json.dumps({ 'searchString' : 'b73',
                    'ignoreCase' : 'true',
                    'autoAndConsecutiveLiterals': 'false',
                    'indexNames' : ['crop_trait_observation'],
                    'resolverName' : 'Accessions',
                    'offset' : 0,
                    'limit' : 10000
                    })
elif type == 'xml':
    # using pox (plain old xml)
    headers = {'Content-Type' : 'text/xml' }
    data = """
<Search xmlns='http://www.grin-global.org/'>
  <searchString>b73</searchString>
  <ignoreCase>true</ignoreCase>
  <autoAndConsecutiveLiterals>false</autoAndConsecutiveLiterals>
  <indexNames>
    <indexName>crop_trait_observation</indexName>
  </indexNames>
  <resolverName>Accessions</resolverName>
  <offset>0</offset>
  <limit>10000</limit>
</Search>
"""
else:
    print('invalid request type.')
    exit()

request = urllib2.Request(url, data, headers)
response = urllib2.urlopen(request)
output = response.read()

print output
