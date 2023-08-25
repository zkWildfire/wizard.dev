import logging
from mnist.constants import MNIST_MEAN, MNIST_STD
from mnist.models.model import IModel
from mnist.evaluate import evaluate
import time
import torch
from torch import optim
from torchvision import datasets, transforms # pyright: ignore[reportMissingTypeStubs]
from torch.utils.data import DataLoader
from typing import Tuple

logger = logging.getLogger()

def train(
	model: IModel,
	device: torch.device,
	epochs: int = 5,
	learning_rate: float = 0.001) -> None:
	"""
	Runs the training loop for the model.
	@param model The model to train.
	@param device The device to train the model on.
	@param epochs The number of epochs to train for.
	@param learning_rate The learning rate to use for training.
	"""
	# Define a transform to normalize the data
	logger.info("Loading training data...")
	transform = transforms.Compose([
		transforms.ToTensor(),
		transforms.Normalize((MNIST_MEAN,), (MNIST_STD,))
	])

	# Download and load the training data
	train_data = datasets.MNIST(
		"../../data",
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

	# Keep track of timing information
	elapsed_time = 0.0

	# Run the training loop
	logger.info("Starting training...")
	for epoch in range(epochs):
		model.train()

		start_time = time.time()
		for images, labels in data_loader:
			# Move the data to the correct device
			images = images.to(device)
			labels = labels.to(device)

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

		duration = time.time() - start_time
		elapsed_time += duration
		# Non-zero indexed epoch
		curr_epoch = epoch + 1
		remaining_epochs = epochs - curr_epoch
		avg_time_per_epoch = elapsed_time / curr_epoch
		estimated_time_remaining = remaining_epochs * avg_time_per_epoch
		logger.info(
			f"Epoch {epoch+1}/{epochs} finished ("
			f"duration: {round(duration, 2)}s, "
			f"estimated time remaining: {round(estimated_time_remaining, 2)}s"
			")."
		)

	logger.info(
		f"Training completed. Total training time: {round(elapsed_time, 2)}s."
	)
	evaluate(model, device)
