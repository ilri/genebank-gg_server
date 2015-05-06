import urllib
dv = "get_accession";
params = ":accessionid=382129;:cooperatorid=;:cropid=;:geographyid=;:inventoryid=;:orderrequestid=;:taxonomygenusid=;";
fin = urllib.urlopen("http://localhost/gringlobal/view.aspx?dv=%s&params=%s" % (dv, params));
fout = open('./output.txt', 'w')
fout.write(fin.read())
