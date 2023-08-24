from mnist.models.model import IModel

class SimpleModel(IModel):
	"""
	A simple model used to test the MNIST training and evaluation scripts.
	"""
	@property
	def model_name(self) -> str:
		"""
		The name of the model.
		"""
		return "simple"
