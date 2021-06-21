# Dupo
A program that finds all files under a directory (including subdirectories), computes an md5 hash of the contents of each file, and finally lists the files grouped by hash.

This tool is to help assist in identifying duplicate files within a directory so the user can better organize their files and reduce wasted hard drive space.

# Usage
Dupo /path/to/directory

## Stretch Goal
Instead of dumping a list, an interactive mode could be useful. Go over the files group by group, and let the user specify what to delete and keep. Maybe even a copyto function, where the original directory is untouched and a new directory is made with files copied from the original - but without duplicates. Ideas.