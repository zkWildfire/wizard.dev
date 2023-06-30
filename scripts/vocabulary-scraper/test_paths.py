from constants import NUM_SECTIONS
from pathlib import Path
from paths import Paths

class MockRepository:
	"""
	Helper class used to set up files on disk for test cases.
	"""

	def __init__(self, path: Path, section_count: int) -> None:
		"""
		Generates a file structure roughly mirroring the repository.
		@param path Path to the temporary folder to generate files in. This
		  folder will act as the root folder for the mock repository.
		@param section_count The number of sections to generate.
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


def test_repo_root(tmp_path: Path):
	"""
	Verifies that `Paths.repo_root` returns the correct path.
	"""
	repo = MockRepository(tmp_path, 1)
	assert Paths(repo.script_dir).repo_root == tmp_path


def test_japanese_root(tmp_path: Path):
	"""
	Verifies that `Paths.japanese_root` returns the correct path.
	"""
	repo = MockRepository(tmp_path, 1)
	assert Paths(repo.script_dir).japanese_root == repo.japanese_dir


def test_vocabulary_file(tmp_path: Path):
	"""
	Verifies that `Paths.vocabulary_file` returns the correct path.
	"""
	repo = MockRepository(tmp_path, 1)
	assert Paths(repo.script_dir).vocabulary_file_path == repo.vocabulary_file


def test_section_paths(tmp_path: Path):
	"""
	Verifies that `Paths.section_paths` returns the correct paths.
	"""
	# The section paths property should return the path to each section folder
	EXPECTED_COUNT = NUM_SECTIONS
	count = 0

	repo = MockRepository(tmp_path, EXPECTED_COUNT)
	for section_path in Paths(repo.script_dir).section_paths:
		assert section_path == repo.japanese_dir / f"section-{count + 1}"
		count += 1

	assert count == EXPECTED_COUNT
