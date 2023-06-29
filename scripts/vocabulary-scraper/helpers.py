from pathlib import Path
from section_unit import SectionUnit
from typing import Generator
from vocabulary_entry import VocabularyEntry

def get_units(section_number: int, section_path: Path) -> \
	Generator[SectionUnit, None, None]:
	"""
	Gets the units in a section.
	@param section_number The number of the section.
	@param section_path The path to the section folder.
	@returns Each unit in the section.
	"""
	# Each section folder will be composed of some number of unit files starting
	#   from unit 1 and going to some number n.
	# Since the exact number of units in a section is unknown, load each unit's
	#   file until a file is not found.
	i = 1
	try:
		while True:
			yield SectionUnit(section_path, section_number, i)
			i += 1
	except FileNotFoundError:
		# If the file is not found, then there are no more units
		pass


def vocabulary_entry_to_string(entry: VocabularyEntry) -> str:
	"""
	Converts a vocabulary entry to a string.
	@param entry Vocabulary entry to convert.
	@returns The vocabulary entry as a string.
	"""
	return f"| {entry.english} | {entry.kana} | {entry.kanji} | " + \
		f"Section {entry.section_number} Unit {entry.unit_number} |"
