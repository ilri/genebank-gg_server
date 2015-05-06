import urllib2
import json

# using pox (plain old xml)

url = 'http://grin-global-test1.agron.iastate.edu:2011/SearchHost/rest/Search'

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

request = urllib2.Request(url, data, headers)
response = urllib2.urlopen(request)
output = response.read()

print output
