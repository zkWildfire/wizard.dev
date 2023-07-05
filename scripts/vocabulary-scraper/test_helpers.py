from helpers import get_units, vocabulary_entry_to_string
from pathlib import Path
from vocabulary_entry import VocabularyEntry

def generate_unit_file(section_path: Path, unit_number: int):
	"""
	Helper method used to create a unit file in a section.
	@param section_path The path to the section folder.
	@param unit_number The number of the unit.
	"""
	unit_path = section_path / f"unit-{unit_number}.md"
	unit_path.touch()


def init_temp_folder(path: Path, section_count: int, unit_count: int):
	"""
	Generates a file structure roughly mirroring the docs' Japanese section.
	@param path Path to the temporary folder to generate files in.
	@param section_count The number of sections to generate.
	@param unit_count The number of units to generate in each section.
	"""
	for i in range(1, section_count + 1):
		section_path = path / f"section-{i}"
		section_path.mkdir()
		for j in range(1, unit_count + 1):
			generate_unit_file(section_path, j)


def test_get_units_with_no_units(tmp_path: Path):
	"""
	Verifies that no results are returned if no units are found.
	"""
	init_temp_folder(tmp_path, 1, 0)
	count = 0
	for _ in get_units(1, tmp_path / "section-1"):
		count += 1

	assert count == 0


def test_get_units_with_single_section(tmp_path: Path):
	"""
	Verifies that `get_units()` returns all units from a single section.
	"""
	EXPECTED_COUNT = 3
	init_temp_folder(tmp_path, 1, EXPECTED_COUNT)
	count = 0
	for _ in get_units(1, tmp_path / "section-1"):
		count += 1

	assert count == EXPECTED_COUNT


def test_get_units_from_multiple_sections(tmp_path: Path):
	"""
	Verifies that `get_units()` returns all units from multiple sections.
	"""
	EXPECTED_COUNT = 3
	SECTION_COUNT = 3
	init_temp_folder(tmp_path, SECTION_COUNT, EXPECTED_COUNT)
	count = 0
	for i in range(1, SECTION_COUNT + 1):
		for _ in get_units(i, tmp_path / f"section-{i}"):
			count += 1

	assert count == (EXPECTED_COUNT * SECTION_COUNT)


def test_vocabulary_entry_to_string():
	"""
	Makes sure that the string contains all expected components.
	"""
	ENGLISH = "foo"
	KANA = "bar"
	KANJI = "baz"
	SECTION_NUMBER = 3
	UNIT_NUMBER = 5
	entry = VocabularyEntry(ENGLISH, KANA, KANJI, SECTION_NUMBER, UNIT_NUMBER)

	entry_str = vocabulary_entry_to_string(entry)
	assert ENGLISH in entry_str
	assert KANA in entry_str
	assert KANJI in entry_str
	assert str(SECTION_NUMBER) in entry_str
	assert str(UNIT_NUMBER) in entry_str


def test_vocabulary_entry_to_string_with_no_kanji():
	"""
	Makes sure that the string contains an empty string for kanji.
	"""
	ENGLISH = "foo"
	KANA = "bar"
	KANJI = None
	SECTION_NUMBER = 3
	UNIT_NUMBER = 5
	entry = VocabularyEntry(ENGLISH, KANA, KANJI, SECTION_NUMBER, UNIT_NUMBER)

	entry_str = vocabulary_entry_to_string(entry)
	assert ENGLISH in entry_str
	assert KANA in entry_str
	assert str(KANJI) not in entry_str
	assert str(SECTION_NUMBER) in entry_str
	assert str(UNIT_NUMBER) in entry_str
