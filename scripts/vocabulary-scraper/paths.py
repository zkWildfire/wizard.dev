from constants import NUM_SECTIONS
from pathlib import Path
from typing import Generator

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
		return Path(__file__).parent.parent.parent.resolve()


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
