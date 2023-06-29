from typing import NamedTuple, Optional

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
