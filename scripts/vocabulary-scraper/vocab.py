#!/usr/bin/env python3
# Usage: ./vocab.py
# This is a helper script that is used only to update the vocabulary list in the
#   docs folder's Japanese section. The script reads each per-unit file and
#   extracts the vocabulary entries from them. It then reads the vocabulary
#   file and updates it with the new entries. If an entry already exists in the
#   vocabulary file, the script will keep the kanji from the existing entry but
#   use the data from the per-unit entry for all other table columns.
from helpers import get_units
from pathlib import Path
from paths import Paths
import sys
from typing import List
from vocabulary_entry import VocabularyEntry
from vocabulary_file import VocabularyFile

def main() -> int:
	"""
	Primary entry point of the script.
	@returns The exit code of the script.
	"""
	# Get the vocabulary entries from each unit
	vocabulary_entries: List[VocabularyEntry] = []
	paths = Paths(Path(__file__).parent)
	for section_number, section_path in enumerate(paths.section_paths, start=1):
		for unit in get_units(section_number, section_path):
			print(f"Processing {unit.unit_name}...")
			vocabulary_entries.extend(unit.get_vocabulary())
	print(f"Found {len(vocabulary_entries)} vocabulary entries.")

	# Sort the vocabulary entries by English word
	vocabulary_entries.sort(key=lambda entry: entry.english.lower())

	# Write the vocabulary entries to the vocabulary file
	print(f"Updating {Paths.vocabulary_file_path}...")
	vocab_file = VocabularyFile(paths.vocabulary_file_path)
	vocab_file.write(vocabulary_entries)

	print("Successfully updated the vocabulary file.")
	return 0


if __name__ == "__main__":
	sys.exit(main())
