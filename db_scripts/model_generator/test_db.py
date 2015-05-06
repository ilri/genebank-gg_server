import sys
import os

def printlasterror():
    print 'FAILED:\n\t'
    err = sys.exc_info()
    for k in err:
        print k,

def test():
    print 'testing Oracle...',
    try:
        import cx_Oracle
        #cn_or = cx_Oracle.connect('TEST_USER', 'passw0rd!', 'XE')
        cn_or = cx_Oracle.connect('nc7bw', 'sunpasswd', 'NPGS')
        cn_or.close()
    except:
        printlasterror()
    else:
        print 'success'
        
    print 'testing MySQL...',
    try:
        import MySQLdb
        cn_my = MySQLdb.connect('localhost', 'test_user', 'passw0rd!')
        cn_my.close()
    except:
        printlasterror()
    else:
        print 'success'
        
    print 'testing Sql Server with pymssql...',
    try:
        import pymssql
        cn_ms = pymssql.connect(user='test_user', password='passw0rd!', host='mbpxp\sqlexpress')
        cn_ms.close()
    except:
        printlasterror()
    else:
        print 'success'

def test_django():
    
        
if __name__ == '__main__':
    test()