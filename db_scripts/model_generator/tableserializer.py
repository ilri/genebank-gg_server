import pickle
import shelve
import os

class TableSerializer(object):
    """Inherit a class from this class and the django-based models.Model
    class to enable dumping a database table's data to the file system.
    If pickle() is used, one file per row is generated.
    If shelve() is used, one file is generated for the whole table.
    Use unpickle() and unshelve(), respectively, to pull data from the file
    system.  This is designed as an easy mechanism for transferring data from one 
    database vendor to another in a vendor-agnostic manner.
    """

    @classmethod
    def pickle(klass):
        """Creates one file per row in the current folder with the naming scheme of:
        <tablename>_<primary_key_value>.pkl
        """
        list = klass.objects.all()
        tablename = klass()._meta.db_table
        for x in list:
            f = open('%s_%s.pkl' % (tablename, x.pk), 'w')
            pickle.dump(x, f)
            f.close()
        
    @classmethod
    def unpickle(klass, autosave=False):
        """Reads all <tablename>_*.pkl files from current folder and loads them
        into a list of objects and returns them.  If autosave is passed as True,
        also attempts to save() each object to the database before returning.
        """
        ret = []
        tablename = klass()._meta.db_table
        allfiles = os.listdir('.')
        txtfiles = [x for x in allfiles if x[:len(tablename)] == tablename and x[-4:] == '.pkl']
        for fil in txtfiles:
            f = open(fil, 'r')
            obj = pickle.load(f)
            f.close()
            if autosave:
                obj.save()
            ret.append(obj)
            os.remove(fil)
        return ret
        
    @classmethod
    def shelve(klass):
        list = klass.objects.all()
        tablename = klass()._meta.db_table
        d = shelve.open('%s.shelf' % tablename)
        for x in list:
            d[str(x.pk)] = x
        d.close()

    @classmethod
    def unshelve(klass, autosave=False):
        tablename = klass()._meta.db_table
        d = shelve.open('%s.shelf' % tablename)
        ret = {}
        for k, v in d.iteritems():
            if autosave:
                v.save()
            # copy over to another dictionary to return, since we're going to close the shevled one
            ret[k] = v
        d.close()
        return ret