import logging
from mnist.constants import MNIST_MEAN, MNIST_STD
from mnist.models.model import IModel
from torchvision import datasets, transforms # pyright: ignore[reportMissingTypeStubs]
from torch.utils.data import DataLoader
import torch
from typing import Tuple

logger = logging.getLogger()

def evaluate(model: IModel, device: torch.device) -> None:
	"""
	Evaluate the specified model.
	@param device The device to evaluate the model on.
	@param model The model to evaluate.
	"""
	# Transformations applied to the test data
	transform = transforms.Compose([
		transforms.ToTensor(),
		transforms.Normalize((MNIST_MEAN,), (MNIST_STD,))
	])

	# Load MNIST test data
	test_data = datasets.MNIST(
		"../../data",
		train=False,
		download=True,
		transform=transform
	)
	test_loader: DataLoader[Tuple[torch.Tensor, torch.Tensor]] = \
		DataLoader(test_data, batch_size=64, shuffle=False)

	# Set the model to evaluation mode
	model.eval()
	correct = 0
	total = 0

	def get_num_correct(ps: torch.Tensor, ls: torch.Tensor) -> int:
		"""
		Gets the number of correct predictions.
		@param ps The predictions.
		@param ls The labels.
		@returns The number of correct predictions.
		"""
		return ps.argmax(dim=1).eq(ls).sum().item() # type: ignore

	# No need to track gradients for evaluation
	with torch.no_grad():
		for data, target in test_loader:
			# Move the data to the correct device
			data = data.to(device)
			target = target.to(device)

			# Run the model
			output = model(data)
			correct += get_num_correct(output, target)
			total += len(target)

	# Calculate accuracy
	accuracy: float = 100 * correct / total
	logger.info(f"Accuracy of model '{model.model_name}': {round(accuracy, 2)}%")
