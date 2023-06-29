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
			self._vocab_entries = VocabularyFile._process_vocab_table(file)
			self._post_vocab_table_lines = \
				VocabularyFile._get_post_vocab_table_lines(file)


	def write(self, new_vocab: Sequence[VocabularyEntry]) -> None:
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
			file.writelines([
				vocabulary_entry_to_string(e) for e in merged_vocab
			])

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
		lines: List[str] = []
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
		while line:
			match = regex.match(line)
			if not match:
				break

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
		lines: List[str] = []
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
