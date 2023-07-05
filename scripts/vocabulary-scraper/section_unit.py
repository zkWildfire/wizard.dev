from pathlib import Path
import re
from typing import Generator, TextIO
from vocabulary_entry import VocabularyEntry

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


	@property
	def unit_name(self) -> str:
		"""
		Gets the name of the unit.
		"""
		return f"Section {self._section_number} Unit {self._unit_number}"


	def get_vocabulary(self) -> Generator[VocabularyEntry, None, None]:
		"""
		Iterates over the unit's file and yields each vocabulary entry.
		@returns Each vocabulary entry in the unit's file.
		"""
		with open(self._file_path, "r", encoding="utf-8") as f:
			# Loop until the end of the file is reached
			while self._skip_to_vocabulary_section(f):
				yield from self._process_english_japanese_table(f)


	def _process_english_japanese_table(self, file: TextIO) -> \
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
			yield from self._process_kana_only_table(file)
		elif "Kanji" in header:
			yield from self._process_kana_kanji_table(file)
		else:
			raise RuntimeError("Invalid table header")


	def _process_kana_only_table(self, file: TextIO) -> \
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


	def _process_kana_kanji_table(self, file: TextIO) -> \
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
		# Note that for the kanji section, the second "\s" is optional since if
		#   the Kanji characters aren't present, all whitespace will have been
		#   consumed by the first "\s+".
		regex = re.compile(r"\|\s+(.+?)\s+\|\s+(.+?)\s+\|\s+(.+?)?\s*\|")

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


	def _skip_to_vocabulary_section(self, file: TextIO) -> bool:
		"""
		Consumes lines until a vocabulary section is reached.
		This method will consume lines until either the entire file has been
		  read or a vocabulary section header is found.
		@param file File to read from.
		@post The file's position will be at the end of the file or on the
		  first line after a vocabulary section header.
		@returns True if a vocabulary section header was found, False if the
		  entire file was read.
		"""
		# Regex used to match vocabulary section headers
		regex = re.compile(r"^#+\s+Vocabulary$")

		# Consume lines from the file until the end of the file is reached or
		#   the vocabulary header regex matches the line
		line = file.readline()
		while line and not regex.match(line):
			line = file.readline()

		return bool(line)
