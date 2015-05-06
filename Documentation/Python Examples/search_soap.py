import urllib
import urllib2

url = 'http://grin-global-test1.agron.iastate.edu:2011/SearchHost'

headers = {'SOAPAction' : 'http://www.grin-global.org/ISearchHost/Search',
           'Content-Type' : 'text/xml; charset=utf-8',
            }

data = """
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <Search xmlns="http://www.grin-global.org/">
      <searchString>b73</searchString>
      <ignoreCase>true</ignoreCase>
      <autoAndConsecutiveLiterals>false</autoAndConsecutiveLiterals>
      <indexNames xmlns:a="http://schemas.microsoft.com/2003/10/Serialization/Arrays"
          xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
        <a:string>crop_trait_observation</a:string>
      </indexNames>
      <resolverName>Accessions</resolverName>
      <offset>0</offset>
      <limit>10000</limit>
    </Search>
  </s:Body>
</s:Envelope>

"""

request = urllib2.Request(url, data, headers)
response = urllib2.urlopen(request)
output = response.read()

print output
