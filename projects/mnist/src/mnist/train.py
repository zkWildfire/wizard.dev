import logging
from mnist.models.model import IModel
import torch
from torch import optim
from torchvision import datasets, transforms # pyright: ignore[reportMissingTypeStubs]
from torch.utils.data import DataLoader
from typing import Tuple

logger = logging.getLogger()

def train(model: IModel, epochs: int = 5, learning_rate: float = 0.001) -> None:
	"""
	Runs the training loop for the model.
	@param model The model to train.
	@param epochs The number of epochs to train for.
	@param learning_rate The learning rate to use for training.
	"""
	# Define a transform to normalize the data
	logger.info("Loading training data...")
	MNIST_MEAN = 0.1307
	MNIST_STD = 0.3081
	transform = transforms.Compose([
		transforms.ToTensor(),
		transforms.Normalize((MNIST_MEAN,), (MNIST_STD,))
	])

	# Download and load the training data
	train_data = datasets.MNIST(
		"../data",
		train=True,
		download=True,
		transform=transform
	)
	data_loader: DataLoader[Tuple[torch.Tensor, torch.Tensor]] = \
		DataLoader(train_data, batch_size=64, shuffle=True)
	logger.info("Finished loading training data.")

	# Optionally define a loss function and optimizer
	# These could also be part of the model itself
	loss_function = torch.nn.CrossEntropyLoss()
	optimizer = optim.Adam(model.parameters(), lr=learning_rate)

	# Run the training loop
	logger.info("Starting training...")
	for epoch in range(epochs):
		model.train()
		for images, labels in data_loader:
			# Zero the gradients
			optimizer.zero_grad()

			# Forward pass
			output = model(images)

			# Compute the loss
			loss = loss_function(output, labels)

			# Backward pass
			loss.backward()

			# Update the weights
			optimizer.step()

		logger.info(f"Epoch {epoch+1}/{epochs} finished.")

	logger.info("Training completed")
