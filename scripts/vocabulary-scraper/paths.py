from constants import NUM_SECTIONS
from pathlib import Path
from typing import Generator

class Paths:
	"""
	Helper class used to get various paths.
	"""
	def __init__(self, starting_path: Path) -> None:
		"""
		Initializes the class.
		@param starting_path Path to derive all other paths from. This should
		  be the path of the folder containing this script when running this
		  set of scripts normally but will be set to a different path when using
		  this class in tests.
		"""
		self._starting_path = starting_path


	@property
	def repo_root(self) -> Path:
		"""
		Gets the path to the repository's root.
		"""
		return Path(self._starting_path).parent.parent.resolve()


	@property
	def japanese_root(self) -> Path:
		"""
		Gets the path to the Japanese section of the docs.
		"""
		return self.repo_root / "docs" / "reference" / "japanese"


	@property
	def vocabulary_file_path(self) -> Path:
		"""
		Gets the path to the vocabulary file to update.
		"""
		return self.japanese_root / "vocabulary.md"


	@property
	def section_paths(self) -> Generator[Path, None, None]:
		"""
		Gets the path to each section folder in the Japanese docs.
		"""
		for i in range(NUM_SECTIONS):
			yield self.japanese_root / f"section-{i + 1}"
