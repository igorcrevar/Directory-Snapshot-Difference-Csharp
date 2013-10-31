Directory-Snapshot-Difference-Csharp
====================================

Utility which can save snapshot of some directory(all sub-directories and files with sizes) and later compare that snapshot with current state of same(or some other) directory. Written in c#, even javascript(nodejs) would be better(multiplatform).

### Usage

#### Save snapshot
``` dirsnap -s snapshot_file_path [-d directory_path] ```

- snapshot_file_path = path where snapshot file will be saved
- directory_path = path that will be examined. Defaults to current path

#### Compare snapshots
``` dirsnap -c snapshot_file_path -s diff_file_path [-d directory_path] [--sortbysize] ```

- snapshot_file_path = path where is old snapshot for comparing
- diff_file_path = path where difference will be saved
- directory_path = directory that will be examined
				 if not specified path to directoty is inside snapshot file(same directory will be compared)
- --sortbysize = sort by size, otherwise differences will be sorted by name

