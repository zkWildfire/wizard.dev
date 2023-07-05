from __future__ import annotations
from helpers import vocabulary_entry_to_string
from pathlib import Path
import re
from typing import List, Sequence, TextIO
from vocabulary_entry import VocabularyEntry

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
			VocabularyFile._skip_vocab_table(file)
			self._post_vocab_table_lines = \
				VocabularyFile._get_post_vocab_table_lines(file)


	def write(self, new_vocab: Sequence[VocabularyEntry]) -> None:
		"""
		Writes the new vocabulary to the vocabulary file.
		@param new_vocab New vocabulary to write to the file.
		"""
		# Open the file and overwrite its contents
		with self._file_path.open("w") as file:
			# Write all lines before the vocabulary table
			file.writelines(self._pre_vocab_table_lines)

			# Write the vocabulary table header
			file.write("| English | Kana | Kanji | Introduced In |\n")
			file.write("|:-------:|:----:|:-----:|:-------------:|\n")

			# Write the vocabulary table
			file.writelines([
				f"{vocabulary_entry_to_string(e)}\n" for e in new_vocab
			])
			file.write("\n")

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
		regex = re.compile(r"^#+ Words$")

		# Consume lines from the file until the end of the file is reached or
		#   the vocabulary header regex matches the line
		lines: List[str] = []
		line = file.readline()
		while line and not regex.match(line):
			lines.append(line)
			line = file.readline()

		# Make sure the last line read in isn't lost since it will be the header
		#   line of the vocabulary table
		if line:
			lines.append(line)

		return lines


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
		lines: List[str] = []
		line = file.readline()
		while line:
			lines.append(line)
			line = file.readline()

		return lines


	@staticmethod
	def _skip_vocab_table(file: TextIO) -> None:
		"""
		Consumes all lines until the vocabulary table has been skipped.
		@param file File to read from.
		@pre The file is at the first line in the vocabulary table.
		@post The file's position will be at the second line after the
		  vocabulary table.
		"""
		# Regex used to match each vocabulary table line
		# Each entry in the vocabulary table will look like this:
		# ```
		# | [English] | [Kana] | [Kanji] | [Introduced in] |
		# ```
		regex = re.compile(
			r"^\|\s+(.*)\s+\|\s+(.*)\s+\|\s+(.*)?\s*\|\s+Section (\d+) Unit (\d+)\s+\|$"
		)

		# Skip the table header line and alignment line
		file.readline()
		file.readline()

		# Consume lines from the file until the end of the file is reached or
		#   the vocabulary table line regex doesn't match the line
		line = file.readline()
		while line and regex.match(line):
			line = file.readline()
