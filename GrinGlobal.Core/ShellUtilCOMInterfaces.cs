using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Runtime.InteropServices.CustomMarshalers;

namespace GrinGlobal.Core {
    [ComImport, Guid("D8F015C0-C278-11CE-A49E-444553540000"), TypeLibType((short)0x1050)]
    public interface IShellDispatch {
        [DispId(0x60020000)]
        object Application { [return: MarshalAs(UnmanagedType.IDispatch)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020000)] get; }
        [DispId(0x60020001)]
        object Parent { [return: MarshalAs(UnmanagedType.IDispatch)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)]
        Folder NameSpace([In, MarshalAs(UnmanagedType.Struct)] object vDir);
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)]
        Folder BrowseForFolder([In] int Hwnd, [In, MarshalAs(UnmanagedType.BStr)] string Title, [In] int Options, [In, Optional, MarshalAs(UnmanagedType.Struct)] object RootFolder);
        [return: MarshalAs(UnmanagedType.IDispatch)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)]
        object Windows();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020005)]
        void Open([In, MarshalAs(UnmanagedType.Struct)] object vDir);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020006)]
        void Explore([In, MarshalAs(UnmanagedType.Struct)] object vDir);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020007)]
        void MinimizeAll();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020008)]
        void UndoMinimizeALL();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020009)]
        void FileRun();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000a)]
        void CascadeWindows();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000b)]
        void TileVertically();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000c)]
        void TileHorizontally();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000d)]
        void ShutdownWindows();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000e)]
        void Suspend();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000f)]
        void EjectPC();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020010)]
        void SetTime();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020011)]
        void TrayProperties();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020012)]
        void Help();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020013)]
        void FindFiles();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020014)]
        void FindComputer();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020015)]
        void RefreshMenu();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020016)]
        void ControlPanelItem([In, MarshalAs(UnmanagedType.BStr)] string bstrDir);
    }

 

 

    [ComImport, TypeLibType((short)0x1040), DefaultMember("Title"), Guid("BBCBDE60-C3FF-11CE-8350-444553540000")]
    public interface Folder {
        [DispId(0)]
        string Title { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)] get; }
        [DispId(0x60020001)]
        object Application { [return: MarshalAs(UnmanagedType.IDispatch)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [DispId(0x60020002)]
        object Parent { [return: MarshalAs(UnmanagedType.IDispatch)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [DispId(0x60020003)]
        Folder ParentFolder { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)] get; }
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)]
        FolderItems Items();
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020005)]
        FolderItem ParseName([In, MarshalAs(UnmanagedType.BStr)] string bName);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020006)]
        void NewFolder([In, MarshalAs(UnmanagedType.BStr)] string bName, [In, Optional, MarshalAs(UnmanagedType.Struct)] object vOptions);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020007)]
        void MoveHere([In, MarshalAs(UnmanagedType.Struct)] object vItem, [In, Optional, MarshalAs(UnmanagedType.Struct)] object vOptions);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020008)]
        void CopyHere([In, MarshalAs(UnmanagedType.Struct)] object vItem, [In, Optional, MarshalAs(UnmanagedType.Struct)] object vOptions);
        [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020009)]
        string GetDetailsOf([In, MarshalAs(UnmanagedType.Struct)] object vItem, [In] int iColumn);
    }


    [ComImport, Guid("744129E0-CBE5-11CE-8350-444553540000"), TypeLibType((short)0x1040)]
    public interface FolderItems : IEnumerable {
        [DispId(0x60020000)]
        int Count { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020000)] get; }
        [DispId(0x60020001)]
        object Application { [return: MarshalAs(UnmanagedType.IDispatch)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [DispId(0x60020002)]
        object Parent { [return: MarshalAs(UnmanagedType.IDispatch)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)]
        FolderItem Item([In, Optional, MarshalAs(UnmanagedType.Struct)] object index);
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "", MarshalTypeRef = typeof(EnumeratorToEnumVariantMarshaler), MarshalCookie = "")]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-4)]
        IEnumerator GetEnumerator();
    }


    [ComImport, TypeLibType((short)0x1040), DefaultMember("Name"), Guid("FAC32C80-CBE4-11CE-8350-444553540000")]
    public interface FolderItem {
        [DispId(0x60020000)]
        object Application { [return: MarshalAs(UnmanagedType.IDispatch)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020000)] get; }
        [DispId(0x60020001)]
        object Parent { [return: MarshalAs(UnmanagedType.IDispatch)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [DispId(0)]
        string Name { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)] set; }
        [DispId(0x60020004)]
        string Path { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)] get; }
        [DispId(0x60020005)]
        object GetLink { [return: MarshalAs(UnmanagedType.IDispatch)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020005)] get; }
        [DispId(0x60020006)]
        object GetFolder { [return: MarshalAs(UnmanagedType.IDispatch)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020006)] get; }
        [DispId(0x60020007)]
        bool IsLink { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020007)] get; }
        [DispId(0x60020008)]
        bool IsFolder { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020008)] get; }
        [DispId(0x60020009)]
        bool IsFileSystem { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020009)] get; }
        [DispId(0x6002000a)]
        bool IsBrowsable { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000a)] get; }
        [DispId(0x6002000b)]
        DateTime ModifyDate { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000b)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000b)] set; }
        [DispId(0x6002000d)]
        int Size { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000d)] get; }
        [DispId(0x6002000e)]
        string Type { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000e)] get; }
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000f)]
        FolderItemVerbs Verbs();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020010)]
        void InvokeVerb([In, Optional, MarshalAs(UnmanagedType.Struct)] object vVerb);
    }



    [ComImport, Guid("1F8352C0-50B0-11CF-960C-0080C7F4EE85"), TypeLibType((short)0x1040)]
    public interface FolderItemVerbs : IEnumerable {
        [DispId(0x60020000)]
        int Count { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020000)] get; }
        [DispId(0x60020001)]
        object Application { [return: MarshalAs(UnmanagedType.IDispatch)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [DispId(0x60020002)]
        object Parent { [return: MarshalAs(UnmanagedType.IDispatch)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)]
        FolderItemVerb Item([In, Optional, MarshalAs(UnmanagedType.Struct)] object index);
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "", MarshalTypeRef = typeof(EnumeratorToEnumVariantMarshaler), MarshalCookie = "")]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-4)]
        IEnumerator GetEnumerator();
    }


    [ComImport, Guid("08EC3E00-50B0-11CF-960C-0080C7F4EE85"), DefaultMember("Name"), TypeLibType((short)0x1040)]
    public interface FolderItemVerb {
        [DispId(0x60020000)]
        object Application { [return: MarshalAs(UnmanagedType.IDispatch)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020000)] get; }
        [DispId(0x60020001)]
        object Parent { [return: MarshalAs(UnmanagedType.IDispatch)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [DispId(0)]
        string Name { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)] get; }
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)]
        void DoIt();
    }

 

 

 


 

 


}
