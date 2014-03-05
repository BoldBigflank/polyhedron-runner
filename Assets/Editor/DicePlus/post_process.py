import os.path
from subprocess import call
import shutil
from sys import argv
from mod_pbxproj import XcodeProject

path = argv[1]

print('post_process.py xcode build path --> ' + path)

print('Unzipping DicePlus.framework')
zfile = "Assets/Plugins/iOS/DicePlus.framework.zip"
if os.path.exists(path + "/DicePlus.framework"):
	shutil.rmtree(path + "/DicePlus.framework")
call(["unzip", "-d", path, zfile])
call(["cp", "Assets/Plugins/iOS/Xcode/Wrapper.h", path])
call(["cp", "Assets/Plugins/iOS/Xcode/Wrapper.mm", path])

project = XcodeProject.Load(path + '/Unity-iPhone.xcodeproj/project.pbxproj')
project.add_other_ldflags("-all_load")
project.add_file(path + "/DicePlus.framework")
project.add_file(path + "/Wrapper.h")
project.add_file(path + "/Wrapper.mm")
project.add_file("System/Library/Frameworks/CoreBluetooth.framework", tree='SDKROOT')
if project.modified:
	project.backup
	project.saveFormat3_2()
	print "Modified Xcode project"
else:
	print "NOT modified Xcode project"
