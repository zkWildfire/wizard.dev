from pathlib import Path
import pytest
from section_unit import SectionUnit
from textwrap import dedent

class MockRepository:
	"""
	Helper class used to set up files on disk for test cases.
	"""
	def __init__(self,
		path: Path,
		section_count: int,
		unit_count: int) -> None:
		"""
		Generates a file structure roughly mirroring the repository.
		@param path Path to the temporary folder to generate files in. This
		  folder will act as the root folder for the mock repository.
		@param section_count The number of sections to generate.
		@param unit_count The number of unit files per section to generate.
		@returns The path to the mock folder "containing" this script. This is
		  the path of the folder that would be the parent folder of this
		  script if this script were in the mock repository instead of the
		  real repository.
		"""
		# Generate the folder containing this script
		self.script_dir = path / "scripts" / "vocabulary-scraper"
		self.script_dir.mkdir(parents=True)

		# Generate the folder containing the docs
		self.docs_dir = path / "docs"
		self.docs_dir.mkdir(parents=True)

		# Generate the folder containing the Japanese docs
		self.japanese_dir = self.docs_dir / "reference" / "japanese"
		self.japanese_dir.mkdir(parents=True)

		# Generate the vocabulary file
		self.vocabulary_file = self.japanese_dir / "vocabulary.md"
		self.vocabulary_file.touch()

		# Generate the section folders
		for i in range(1, section_count + 1):
			section_dir = self.japanese_dir / f"section-{i}"
			section_dir.mkdir()

			# Generate the unit files
			for j in range(1, unit_count + 1):
				unit_file = section_dir / f"unit-{j}.md"
				unit_file.touch()


def test_ctor_throws_if_unit_file_does_not_exist(tmp_path: Path):
	"""
	Verifies that constructing a section unit instance throws an exception if
	  the unit file does not exist.
	"""
	repo = MockRepository(tmp_path, 1, 1)
	section_path = repo.japanese_dir / "section-1"
	with pytest.raises(FileNotFoundError):
		# Attempt to create a section unit instance for unit 2 despite only
		#   adding one unit per section
		SectionUnit(section_path, 1, 2)


def test_unit_name_contains_expected_components(tmp_path: Path):
	"""
	Verifies that the unit name contains the expected components.
	"""
	section_number = 1
	unit_number = 2

	repo = MockRepository(tmp_path, section_number, unit_number)
	section_path = repo.japanese_dir / f"section-{section_number}"
	unit = SectionUnit(section_path, section_number, unit_number)
	assert "section" in unit.unit_name.lower()
	assert str(section_number) in unit.unit_name
	assert "unit" in unit.unit_name.lower()
	assert str(unit_number) in unit.unit_name


def test_get_vocabulary_from_unit_with_single_kana_only_table(tmp_path: Path):
	"""
	Verifies that the section unit class can extract vocabulary entries from
	  a unit file that contains a single table with kana only.
	Earlier units use a table that only contains a column for kana and does not
	  contain a column for kanji. This test verifies that the section unit
	  class can extract vocabulary entries from such a table.
	"""
	section_number = 1
	unit_number = 1
	english = "foo"
	kana = "bar"

	# Set up the repository
	repo = MockRepository(tmp_path, section_number, unit_number)
	section_path = repo.japanese_dir / f"section-{section_number}"
	unit_path = section_path / f"unit-{unit_number}.md"
	file_contents = dedent(f"""
		# Section 1 Unit 1
		## Not a Vocab Header
		lore ipsum dolor sit amet

		## Vocabulary
		| English | Japanese |
		|:-------:|:--------:|
		| {english} | {kana} |
	""")
	unit_path.write_text(file_contents)

	# Process the unit file
	unit = SectionUnit(section_path, section_number, unit_number)
	vocabulary = list(unit.get_vocabulary())

	# Verify that the vocabulary contains the expected entries
	assert len(vocabulary) == 1
	assert vocabulary[0].english == english
	assert vocabulary[0].kana == kana
	assert vocabulary[0].kanji is None
	assert vocabulary[0].section_number == section_number
	assert vocabulary[0].unit_number == unit_number


def test_get_vocabulary_from_unit_with_single_kana_kanji_table(tmp_path: Path):
	"""
	Verifies that the section unit class can extract vocabulary entries from
	  a unit file that contains a single table with columns for kana and kanji.
	"""
	section_number = 1
	unit_number = 1
	english = "foo"
	kana = "bar"
	kanji = "baz"

	# Set up the repository
	repo = MockRepository(tmp_path, section_number, unit_number)
	section_path = repo.japanese_dir / f"section-{section_number}"
	unit_path = section_path / f"unit-{unit_number}.md"
	file_contents = dedent(f"""
		# Section 1 Unit 1
		## Not a Vocab Header
		lore ipsum dolor sit amet

		## Vocabulary
		| English | Kana | Kanji |
		|:-------:|:----:|:-----:|
		| {english} | {kana} | {kanji} |
	""")
	unit_path.write_text(file_contents)

	# Process the unit file
	unit = SectionUnit(section_path, section_number, unit_number)
	vocabulary = list(unit.get_vocabulary())

	# Verify that the vocabulary contains the expected entries
	assert len(vocabulary) == 1
	assert vocabulary[0].english == english
	assert vocabulary[0].kana == kana
	assert vocabulary[0].kanji == kanji
	assert vocabulary[0].section_number == section_number
	assert vocabulary[0].unit_number == unit_number


def test_get_vocabulary_from_unit_with_multiple_kana_only_tables(tmp_path: Path):
	"""
	Verifies that the section unit class can extract vocabulary entries from
	  a unit file that contains multiple tables with kana only.
	"""
	section_number = 2
	unit_number = 3
	english = "foo"
	kana = "bar"

	# Set up the repository
	repo = MockRepository(tmp_path, section_number, unit_number)
	section_path = repo.japanese_dir / f"section-{section_number}"
	unit_path = section_path / f"unit-{unit_number}.md"
	file_contents = dedent(f"""
		# Section 2 Unit 3
		## Not a Vocab Header
		lore ipsum dolor sit amet

		## Vocabulary
		| English | Japanese |
		|:-------:|:--------:|
		| {english}1 | {kana}1 |

		## Vocabulary
		| English | Japanese |
		|:-------:|:--------:|
		| {english}2 | {kana}2 |

		## Vocabulary
		| English | Japanese |
		|:-------:|:--------:|
		| {english}3 | {kana}3 |
	""")
	unit_path.write_text(file_contents)

	# Process the unit file
	unit = SectionUnit(section_path, section_number, unit_number)
	vocabulary = list(unit.get_vocabulary())

	# Verify that the vocabulary contains the expected entries
	assert len(vocabulary) == 3
	for i, entry in enumerate(vocabulary, start=1):
		assert entry.english == f"{english}{i}"
		assert entry.kana == f"{kana}{i}"
		assert entry.kanji is None
		assert entry.section_number == section_number
		assert entry.unit_number == unit_number


def test_get_vocabulary_from_unit_with_multiple_kana_kanji_tables(tmp_path: Path):
	"""
	Verifies that the section unit class can extract vocabulary entries from
	  a unit file that contains multiple tables with columns for kana and kanji.
	"""
	section_number = 2
	unit_number = 3
	english = "foo"
	kana = "bar"
	kanji = "baz"

	# Set up the repository
	repo = MockRepository(tmp_path, section_number, unit_number)
	section_path = repo.japanese_dir / f"section-{section_number}"
	unit_path = section_path / f"unit-{unit_number}.md"
	file_contents = dedent(f"""
		# Section 2 Unit 3
		## Not a Vocab Header
		lore ipsum dolor sit amet

		## Vocabulary
		| English | Kana | Kanji |
		|:-------:|:----:|:-----:|
		| {english}1 | {kana}1 | {kanji}1 |

		## Vocabulary
		| English | Kana | Kanji |
		|:-------:|:----:|:-----:|
		| {english}2 | {kana}2 | {kanji}2 |

		## Vocabulary
		| English | Kana | Kanji |
		|:-------:|:----:|:-----:|
		| {english}3 | {kana}3 | {kanji}3 |
	""")
	unit_path.write_text(file_contents)

	# Process the unit file
	unit = SectionUnit(section_path, section_number, unit_number)
	vocabulary = list(unit.get_vocabulary())

	# Verify that the vocabulary contains the expected entries
	assert len(vocabulary) == 3
	for i, entry in enumerate(vocabulary, start=1):
		assert entry.english == f"{english}{i}"
		assert entry.kana == f"{kana}{i}"
		assert entry.kanji == f"{kanji}{i}"
		assert entry.section_number == section_number
		assert entry.unit_number == unit_number


def test_get_vocabulary_from_mixed_file(tmp_path: Path):
	"""
	Verifies that the section unit class can extract vocabulary entries from
	  a unit file that contains both kana-only and kana-kanji tables.
	"""
	section_number = 2
	unit_number = 3
	english = "foo"
	kana = "bar"
	kanji = "baz"

	# Set up the repository
	repo = MockRepository(tmp_path, section_number, unit_number)
	section_path = repo.japanese_dir / f"section-{section_number}"
	unit_path = section_path / f"unit-{unit_number}.md"
	file_contents = dedent(f"""
		# Section 2 Unit 3
		## Not a Vocab Header
		lore ipsum dolor sit amet

		## Vocabulary
		| English | Kana | Kanji |
		|:-------:|:----:|:-----:|
		| {english}1 | {kana}1 | {kanji}1 |

		## Vocabulary
		| English | Japanese |
		|:-------:|:--------:|
		| {english}2 | {kana}2 |

		## Vocabulary
		| English | Kana | Kanji |
		|:-------:|:----:|:-----:|
		| {english}3 | {kana}3 | {kanji}3 |

		## Vocabulary
		| English | Japanese |
		|:-------:|:--------:|
		| {english}4 | {kana}4 |
	""")
	unit_path.write_text(file_contents)

	# Process the unit file
	unit = SectionUnit(section_path, section_number, unit_number)
	vocabulary = list(unit.get_vocabulary())

	# Verify that the vocabulary contains the expected entries
	assert len(vocabulary) == 4
	for i, entry in enumerate(vocabulary, start=1):
		assert entry.english == f"{english}{i}"
		assert entry.kana == f"{kana}{i}"
		assert entry.kanji == (f"{kanji}{i}" if i % 2 == 1 else None)
		assert entry.section_number == section_number
		assert entry.unit_number == unit_number
