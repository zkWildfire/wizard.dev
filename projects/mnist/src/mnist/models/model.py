from abc import ABC, abstractmethod
from pathlib import Path
import torch

class IModel(ABC, torch.nn.Module):
	"""
	Base type used for classes that implement models for MNIST classification.
	"""
	@property
	@abstractmethod
	def model_name(self) -> str:
		"""
		The name of the model.
		"""
		raise NotImplementedError()


	def forward(self, x: torch.Tensor) -> torch.Tensor:
		"""
		Runs the forward pass of the model.
		@param x The input tensor.
		@returns The normalized output tensor that specifies the probability
		  that the input belongs to each class.
		"""
		# To be implemented by child classes
		raise NotImplementedError()


	def save_model(self, path: Path) -> None:
		"""
		Saves the model to a file on disk.
		@param path The path to save the model to.
		"""
		torch.save(self.state_dict(), path) # pyright: ignore[reportUnknownMemberType]


	def load_model(self, path: Path) -> None:
		"""
		Loads the model from a file on disk.
		This will override whatever weights the model is currently using.
		@param path The path to load the model from.
		"""
		state_dict = torch.load(path) # pyright: ignore[reportUnknownMemberType]
		self.load_state_dict(state_dict)
