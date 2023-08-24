from abc import ABC, abstractmethod

class IModel(ABC):
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
