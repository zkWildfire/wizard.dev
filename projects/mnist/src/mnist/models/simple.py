from mnist.constants import *
from mnist.models.model import IModel
import torch
import torch.nn.functional as F

class SimpleModel(IModel):
	"""
	A simple model used to test the MNIST training and evaluation scripts.
	"""
	def __init__(self):
		"""
		Initializes a new instance of the class.
		"""
		super(IModel, self).__init__() # pyright: ignore[reportUnknownMemberType]

		# Arbitrarily selected hidden size
		HIDDEN_SIZE = 50
		self._fc1 = torch.nn.Linear(INPUT_SIZE, HIDDEN_SIZE)
		self._fc2 = torch.nn.Linear(HIDDEN_SIZE, NUM_CLASSES)


	@property
	def model_name(self) -> str:
		"""
		The name of the model.
		"""
		return "simple"


	def forward(self, x: torch.Tensor) -> torch.Tensor:
		"""
		Runs the forward pass of the model.
		@param x The input tensor.
		@returns The normalized output tensor that specifies the probability
		  that the input belongs to each class.
		"""
		# Flatten the input
		x = x.view(-1, INPUT_SIZE)

		# Process the tensor
		x = F.relu(self._fc1(x))
		x = self._fc2(x)
		return F.log_softmax(x, dim=1)
