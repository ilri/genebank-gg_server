root = "\projects\gringlobal"

set a = wscript.arguments
newVer = ""
if a.count = 0 then 
	newVer = InputBox("Enter the new version number (e.g. 1.5.874):")
else 
	newVer = a(0)
end if

if newVer = "" then
	wscript.echo "quitting, no version provided"
	wscript.quit 1
end if

set fso = CreateObject("Scripting.FileSystemObject")

excludeFolders = Array("GRINGlobal.Client", "TestApps", "WindowsFormsApplication1", ".svn", "_svn")

ret = scanFolders(root, newVer, excludeFolders)
wscript.echo "Updated the following files: " & ret

function scanFolders(parent, newVersion, excludeFolders)
	touched = ""
	for each fil in fso.getfolder(parent).files
		if (lcase(fso.getfilename(fil)) = "assemblyinfo.cs") then
			'wscript.echo "munging assembly file " & fil
			touched = touched & replaceAssemblyVersion(fil, newVersion)
		elseif (lcase(fso.getextensionname(fil)) = "vdproj") then
			'wscript.echo "munging setup file " & fil
			touched = touched & replaceSetupVersion(fil, newVersion)
		end if
	next
	for each fldr in fso.getfolder(parent).subfolders
		skip = false
		for each exFldr in excludeFolders
			if (instr(fldr, exFldr) > 0) then
				' skip, is exclude folder
				skip = true
			end if
		next
		if not skip then
			touched = touched & scanFolders(fldr, newVersion, excludeFolders)
		end if
	next
	scanFolders = touched
end function

wscript.quit 0

function backupFile(fn)
	'read and backup project file
	set f = fso.OpenTextFile(fn) ' (fn, 1, false, -1) ' 1=readonly, false = do not create, -1 = unicode (-2=system default, 0=ascii)
	contents = f.ReadAll
	f.Close
	fbak = fn & ".bak"
	if fso.fileexists(fbak) then 
		fso.deletefile fbak
	end if
	fso.copyfile fn, fbak
	backupFile = contents
end function

function replaceAssemblyVersion(f, newVersion)

	contents = backupFile(f)
	

	'find, increment and replace version number
	set re = new regexp
	re.global = true
	re.pattern = "\[assembly: AssemblyVersion\(""(\d+(\.[0123456789\*]+)+)""\)\]"
	contents = re.replace(contents, "[assembly: AssemblyVersion(""" & newVersion & """)]")
	
	set re2 = new regexp
	re2.global = true
	re2.pattern = "\[assembly: AssemblyFileVersion\(""(\d+(\.[0123456789\*]+)+)""\)\]"
	contents = re2.replace(contents, "[assembly: AssemblyFileVersion(""" & newVersion & """)]")
	
	replaceAssemblyVersion = "Assembly--" & saveFile(f, contents)
	
end function

function replaceSetupVersion(f, newVersion)

	contents = backupFile(f)

	'find, increment and replace version number
	set re = new regexp
	re.global = true
	re.pattern = "(""ProductVersion"" = ""8:)(\d+(\.\d+)+)"""
	contents = re.replace(contents, "$1" & newVersion & """")

	'replace ProductCode
	'wscript.echo "Creating new product code..."
	re.pattern = "(""ProductCode"" = ""8:)(\{.+\})"""
	guid = CreateObject("Scriptlet.TypeLib").Guid
	guid = left(guid, len(guid) - 2)
	contents = re.replace(contents, "$1" & guid & """")

	'replace PackageCode
	'wscript.echo "Creating new package code..."
	re.pattern = "(""PackageCode"" = ""8:)(\{.+\})"""
	guid = CreateObject("Scriptlet.TypeLib").Guid
	guid = left(guid, len(guid) - 2)
	contents = re.replace(contents, "$1" & guid & """")

	replaceSetupVersion = "SetupFile--" & saveFile(f, contents)
	
end function

function saveFile(fn1, contents)
	set f = fso.CreateTextfile(fn1, true)  ' true = overwrite if exists, a second true = unicode
	f.write(contents)
	f.close
	saveFile = fn1 & chr(13) & chr(10)
end function