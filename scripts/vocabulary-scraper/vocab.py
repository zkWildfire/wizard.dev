#!/usr/bin/env python3
# Usage: ./vocab.py
# This is a quick and dirty helper script that is used only to update the
#   vocabulary list in the docs folder's Japanese section. As such, the script
#   does not accept any parameters, is contained in a single script, and does
#   not have any unit/integration tests.
from pathlib import Path
import re
import sys
from typing import Generator, List, Optional, NamedTuple, Sequence, TextIO

#
# Miscellaneous Constants
#

# Number of sections in the Japanese docs section.
NUM_SECTIONS = 2

#
# Helper Classes
#
class Paths:
	"""
	"Static" class used to get various paths.
	"""
	@classmethod
	@property
	def repo_root(cls) -> Path:
		"""
		Gets the path to the repository's root.
		"""
		return Path(__file__).parent.parent.resolve()


	@classmethod
	@property
	def japanese_root(cls) -> Path:
		"""
		Gets the path to the Japanese section of the docs.
		"""
		return cls.repo_root / "docs" / "reference" / "japanese"


	@classmethod
	@property
	def vocabulary_file_path(cls) -> Path:
		"""
		Gets the path to the vocabulary file to update.
		"""
		return cls.japanese_root / "vocabulary.md"


	@classmethod
	@property
	def section_paths(cls) -> Generator[Path, None, None]:
		"""
		Gets the path to each section folder in the Japanese docs.
		"""
		for i in range(NUM_SECTIONS):
			yield cls.japanese_root / f"section-{i + 1}"


class VocabularyEntry(NamedTuple):
	"""
	Represents a vocabulary entry.
	"""
	# The English word.
	english: str

	# The Japanese word in kana characters.
	kana: str

	# The Japanese word in kanji characters.
	kanji: Optional[str]

	# The section number that the entry is in.
	section_number: int

	# The unit number that the entry is in.
	unit_number: int


class SectionUnit:
	"""
	Represents a unit within a section.
	"""
	def __init__(self,
		section_path: Path,
		section_number: int,
		unit_number: int) -> None:
		"""
		Initializes a new instance of the class.
		@param section_path The path to the section folder containing the unit.
		@param section_number The number of the section.
		@param unit_number The number of the unit.
		@throws FileNotFoundError If the unit file does not exist.
		"""
		self._section_number = section_number
		self._unit_number = unit_number
		self._file_path = section_path / f"unit-{unit_number}.md"
		if not self._file_path.exists():
			raise FileNotFoundError(f"Unit file not found: {self._file_path}")


	def get_vocabulary(self) -> Generator[VocabularyEntry, None, None]:
		"""
		Iterates over the unit's file and yields each vocabulary entry.
		@returns Each vocabulary entry in the unit's file.
		"""
		with open(self._file_path, "r", encoding="utf-8") as f:
			# Loop until the end of the file is reached
			while SectionUnit._skip_to_vocabulary_section(f):
				yield from SectionUnit._process_english_japanese_table(f)


	def _process_english_japanese_table(self, file: TextIO[str]) -> \
		Generator[VocabularyEntry, None, None]:
		"""
		Processes the English-Japanese table in the unit's file.
		@param file File to read from. Must not be at the end of the file.
		@pre The file is at the first line in the table.
		@post The file's position will be on the second line after the table.
		@returns Each vocabulary entry in the table.
		"""
		# Figure out what kind of table is in the file
		# Files will either contain a table for English + Kana (earlier units)
		#   or English + Kana + Kanji. The English + Kana tables will have
		#   a header like this:
		# ```
		# | English | Japanese |
		# |:-------:|:--------:|
		# ```
		# The English + Kana + Kanji tables will have a header like this:
		# ```
		# | English | Kana | Kanji |
		# |:-------:|:----:|:-----:|
		# ```

		header = file.readline()
		# Always ignore the next line no matter what kind of table is being
		#   processed
		file.readline()

		if "Japanese" in header:
			yield from SectionUnit._process_kana_only_table(file)
		elif "Kanji" in header:
			yield from SectionUnit._process_kana_kanji_table(file)
		else:
			raise RuntimeError("Invalid table header")


	def _process_kana_only_table(self, file: TextIO[str]) -> \
		Generator[VocabularyEntry, None, None]:
		"""
		Processes the Kana-only table in the unit's file.
		@param file File to read from. Must not be at the end of the file.
		@pre The file is at the first line in the table.
		@post The file's position will be on the second line after the table.
		@returns Each vocabulary entry in the table.
		"""
		# Regex used to process the table
		# Each table line will be in the format:
		# | [one or more english words] | [japanese characters] |
		regex = re.compile(r"\|\s+(.+?)\s+\|\s+(.+?)\s+\|")

		# Loop until the end of the table is reached
		while True:
			line = file.readline()

			# If the end of the file is reached, stop
			if line == "":
				break

			match = regex.match(line)
			if match is None:
				# If the line does not match the regex, then the end of the
				#   table has been reached
				break

			# Extract the English and Japanese words from the line
			english = match.group(1)
			japanese = match.group(2)

			# Yield the vocabulary entry
			yield VocabularyEntry(
				english,
				japanese,
				# Kana-only tables do not have kanji
				None,
				self._section_number,
				self._unit_number
			)


	def _process_kana_kanji_table(self, file: TextIO[str]) -> \
		Generator[VocabularyEntry, None, None]:
		"""
		Processes the English-Kana-Kanji table in the unit's file.
		@param file File to read from. Must not be at the end of the file.
		@pre The file is at the first line in the table.
		@post The file's position will be on the second line after the table.
		@returns Each vocabulary entry in the table.
		"""
		# Regex used to process the table
		# Each table line will be in the format:
		# | [one or more english words] | [kana characters] | [kanji characters] |
		regex = re.compile(r"\|\s+(.+?)\s+\|\s+(.+?)\s+\|\s+(.+?)\s+\|")

		# Loop until the end of the table is reached
		while True:
			line = file.readline()

			# If the end of the file is reached, stop
			if line == "":
				break

			match = regex.match(line)
			if match is None:
				# If the line does not match the regex, then the end of the
				#   table has been reached
				break

			# Extract the English, Kana, and Kanji words from the line
			english = match.group(1)
			kana = match.group(2)
			kanji = match.group(3)

			# Yield the vocabulary entry
			yield VocabularyEntry(
				english,
				kana,
				kanji,
				self._section_number,
				self._unit_number
			)


	def _skip_to_vocabulary_section(self, file: TextIO[str]) -> bool:
		"""
		Consumes lines until a vocabulary section is reached.
		This method will consume lines until either the entire file has been
		  read or a vocabulary section header is found.
		@param file File to read from.
		@post The file's position will be at the end of the file or on the
		  second line after a vocabulary section header.
		@returns True if a vocabulary section header was found, False if the
		  entire file was read.
		"""
		# Regex used to match vocabulary section headers
		regex = re.compile(r"^#+Vocabulary$")

		# Consume lines from the file until the end of the file is reached or
		#   the vocabulary header regex matches the line
		line = file.readline()
		while line and not regex.match(line):
			line = file.readline()

		return bool(line)


class VocabularyFile:
	"""
	Helper class for managing the main vocabulary file.
	This class is responsible for overwriting the previous vocabulary file with
	  the new vocabulary file.
	"""
	def __init__(self, vocab_file_path: Path) -> None:
		"""
		Initializes the class.
		@param vocab_file_path Path to the vocabulary file.
		"""
		self._file_path = vocab_file_path

		# Get all the lines from the file that should not be overwritten
		with vocab_file_path.open("r") as file:
			self._pre_vocab_table_lines = \
				VocabularyFile._get_pre_vocab_table_lines(file)
			self._vocab_entries = VocabularyFile._skip_vocab_table(file)
			self._post_vocab_table_lines = \
				VocabularyFile._get_post_vocab_table_lines(file)


	def write(self, new_vocab: Sequence[str]) -> None:
		"""
		Writes the new vocabulary to the vocabulary file.
		@param new_vocab New vocabulary to write to the file.
		"""
		# Merge the vocab entries from the vocabulary files with the vocab
		#   entries read from the vocab file
		# This is necessary because the vocab file may contain entries that
		#   have kanji added, whereas the unit files may not contain the kanji
		#   characters for those entries since many words are introduced via
		#   kana only.
		# Note that both the vocab entries from the vocabulary file and the
		#   vocab entries from the unit files are sorted by English word, but
		#   the from-units vocab entries may have extra entries not present in
		#   the vocabulary file.
		merged_vocab = VocabularyFile._merge_vocab_entries(
			self._vocab_entries,
			new_vocab
		)

		# Open the file and overwrite its contents
		with self._file_path.open("w") as file:
			# Write all lines before the vocabulary table
			file.writelines(self._pre_vocab_table_lines)

			# Write the vocabulary table header
			file.write("| English | Kana | Kanji | Introduced In |")
			file.write("|:-------:|:----:|:-----:|:-------------:|")

			# Write the vocabulary table
			file.writelines(merged_vocab)

			# Write all lines after the vocabulary table
			file.writelines(self._post_vocab_table_lines)


	@staticmethod
	def _get_pre_vocab_table_lines(file: TextIO) -> List[str]:
		"""
		Gets the lines in the vocabulary file before the vocabulary table.
		@param file File to read from.
		@pre The file is at the first line in the file.
		@post The file's position will be on the table header line of the
		  vocabulary table.
		@returns The lines before the vocabulary table.
		"""
		# Regex used to match the vocabulary table header
		regex = re.compile(r"^#+Words$")

		# Consume lines from the file until the end of the file is reached or
		#   the vocabulary header regex matches the line
		lines = []
		line = file.readline()
		while line and not regex.match(line):
			lines.append(line)
			line = file.readline()

		return lines


	@staticmethod
	def _process_vocab_table(file: TextIO) -> List[VocabularyEntry]:
		"""
		Processes each line belonging to the vocabulary table.
		@param file File to read from.
		@pre The file is at the first line in the vocabulary table.
		@post The file's position will be on the second line after the
		  vocabulary table.
		@returns A list of vocabulary entries read in from the file.
		"""
		# Regex used to match each vocabulary table line
		# Each entry in the vocabulary table will look like this:
		# ```
		# | [English] | [Kana] | [Kanji] | [Introduced in] |
		# ```
		regex = re.compile(
			r"^\|\s+(.*)\s+\|\s+(.*)\s+\|\s+(.*)\s+\|\s+Section (\d+) Unit (\d+)\s+\|$"
		)

		entries: List[VocabularyEntry] = []
		line = file.readline()
		while line and regex.match(line):
			match = regex.match(line)

			# Extract the English, Kana, and Kanji words from the line
			english = match.group(1)
			kana = match.group(2)
			kanji = match.group(3)

			# Extract the section and unit numbers from the line
			section_number = int(match.group(4))
			unit_number = int(match.group(5))

			# Create the vocabulary entry
			entries.append(VocabularyEntry(
				english,
				kana,
				kanji,
				section_number,
				unit_number
			))

			line = file.readline()

		return entries


	@staticmethod
	def _get_post_vocab_table_lines(file: TextIO) -> List[str]:
		"""
		Gets the lines in the vocabulary file after the vocabulary table.
		@param file File to read from.
		@pre The file is at the first line after the vocabulary table.
		@post The file's position will be at the end of the file.
		@returns The lines after the vocabulary table.
		"""
		# Consume lines from the file until the end of the file is reached
		lines = []
		line = file.readline()
		while line:
			lines.append(line)
			line = file.readline()

		return lines


	@staticmethod
	def _merge_vocab_entries(
		existing_vocab: Sequence[VocabularyEntry],
		new_vocab: Sequence[VocabularyEntry]) -> List[VocabularyEntry]:
		"""
		Merges the new vocabulary entries with the existing vocabulary entries.
		This method will keep all data from the new vocabulary entries except
		  the kanji field, which allows kanji to be added to the vocabulary
		  file as new kanji characters are introduced.
		@param existing_vocab Vocabulary entries generated from the vocabulary
		  file.
		@param new_vocab Vocabulary entries generated from the unit files. This
		  sequence may be longer than the existing vocabulary entries since
		  the unit files may contain entries that are not present in the
		  vocabulary file yet.
		"""
		merged_vocab: List[VocabularyEntry] = []

		# Keep track of the index of the next entry in the existing vocabulary
		#   to compare against
		existing_vocab_index = 0

		# Process each entry in the new vocabulary
		for new_entry in new_vocab:
			existing_entry = existing_vocab[existing_vocab_index]

			# If the new entry is the same as the existing entry, keep the
			#   kanji from the existing entry but keep the other data from the
			#   new entry
			# Note that since the new vocabulary entry's kanji field may be
			#   overwritten, it is not used in the comparison to decide whether
			#   the existing entry is a match for the new entry
			merged_entry = new_entry
			if new_entry.english == existing_entry.english and \
				new_entry.kana == existing_entry.kana:
				merged_entry = VocabularyEntry(
					new_entry.english,
					new_entry.kana,
					existing_entry.kanji,
					new_entry.section_number,
					new_entry.unit_number
				)

				# Move to the next entry in the existing vocabulary since
				#   the existing entry was used
				existing_vocab_index += 1

			merged_vocab.append(merged_entry)

		# All entries in the existing vocabulary should have been used
		assert existing_vocab_index == len(existing_vocab)
		return merged_vocab

#
# Primary Script Logic
#

def vocabulary_entry_to_string(entry: VocabularyEntry) -> str:
	"""
	Converts a vocabulary entry to a string.
	@param entry Vocabulary entry to convert.
	@returns The vocabulary entry as a string.
	"""
	return f"| {entry.english} | {entry.kana} | {entry.kanji} | " + \
		f"Section {entry.section_number} Unit {entry.unit_number} |"


def get_units(section_path: Path) -> Generator[SectionUnit, None, None]:
	"""
	Gets the units in a section.
	"""
	# Each section folder will be composed of some number of unit files starting
	#   from unit 1 and going to some number n.
	# Since the exact number of units in a section is unknown, load each unit's
	#   file until a file is not found.
	i = 1
	try:
		while True:
			yield SectionUnit(section_path, i)
			i += 1
	except FileNotFoundError:
		# If the file is not found, then there are no more units
		pass


def main() -> int:
	"""
	Primary entry point of the script.
	@returns The exit code of the script.
	"""
	# Get the vocabulary entries from each unit
	vocabulary_entries: List[VocabularyEntry] = []
	for section_path in Paths.section_paths:
		for unit in get_units(section_path):
			vocabulary_entries.extend(unit.get_vocabulary())

	# Sort the vocabulary entries by English word
	vocabulary_entries.sort(key=lambda entry: entry.english.lower())

	# Convert each vocabulary entry to a string
	vocabulary_entries = [
		vocabulary_entry_to_string(entry) for entry in vocabulary_entries
	]

	# Write the vocabulary entries to the vocabulary file
	vocab_file = VocabularyFile(Paths.vocab_file_path)
	vocab_file.write(vocabulary_entries)


if __name__ == "__main__":
	sys.exit(main())
