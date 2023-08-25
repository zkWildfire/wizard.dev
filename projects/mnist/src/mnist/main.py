#!/usr/bin/env python3
# Entry point for MNIST model training and evaluation.
import argparse
import logging
from mnist.models.model import IModel
from mnist.models.simple import SimpleModel
from mnist.evaluate import evaluate
from mnist.train import train
import torch
from typing import Callable, List, Dict
import sys

# Log levels that may be specified on the command line
LOG_LEVELS: Dict[str, int] = {
	"critical": logging.CRITICAL,
	"error": logging.ERROR,
	"warning": logging.WARNING,
	"info": logging.INFO,
	"debug": logging.DEBUG
}


# Supported models with corresponding factory methods
MODELS: Dict[str, Callable[[], IModel]] = {
	"simple": lambda: SimpleModel()
}


class CliArgs(argparse.Namespace):
	"""
	Represents the parsed command line arguments.
	"""
	# Command that was selected
	command: str

	# Minimum log level to print to the console
	log_level: str

	# The model or models that were selected
	# For the "train" command, this will always be a single model. For the
	#   "evaluate" command, this will be a string or list of models.
	model: str | List[str]


class LogFormatter(logging.Formatter):
	"""
	Formatter used for logging messages.
	"""
	# Mapping of log levels to the single character used to represent them.
	LOG_LEVEL_MAPPING = {
		'DEBUG': 'D',
		'INFO': 'I',
		'WARNING': 'W',
		'ERROR': 'E',
		'CRITICAL': 'C'
	}

	def format(self, record: logging.LogRecord) -> str:
		"""
		Re-maps the log level name to a single character.
		@param record The log record to format.
		@returns The formatted log record.
		"""
		record.levelname = LogFormatter.LOG_LEVEL_MAPPING.get(
			record.levelname,
			record.levelname[0]
		)
		return super().format(record)


def make_parser() -> argparse.ArgumentParser:
	"""
	Creates the argument parser for the MNIST training and evaluation.
	@returns The argument parser for the commands.
	"""
	parser = argparse.ArgumentParser(
		description="MNIST model training and evaluation."
	)
	parser.add_argument(
		"--log-level",
		type=str,
		choices=LOG_LEVELS.keys(),
		default="info",
		help="The minimum log level to print to the console."
	)
	subparsers = parser.add_subparsers(dest="command")

	train_parser = subparsers.add_parser("train", help="Train a model.")
	train_parser.add_argument(
		"--model",
		type=str,
		choices=MODELS.keys(),
		required=True,
		help="The model to train."
	)

	evaluate_parser = subparsers.add_parser(
		"evaluate",
		help="Evaluate one or more models."
	)
	evaluate_parser.add_argument(
		"--model",
		type=str,
		choices=MODELS.keys(),
		nargs='+',
		required=True,
		help="The models to evaluate."
	)

	return parser


def main(*cli_args: str) -> int:
	"""
	Entry point for MNIST model training and evaluation.
	@param cli_args The command line arguments to parse. Should not include the
	  script name.
	"""
	# Process command line arguments
	parser = make_parser()
	args = parser.parse_args(cli_args, namespace=CliArgs())

	# Set up logging
	logger = logging.getLogger()
	logger.setLevel(LOG_LEVELS[args.log_level])

	# Configure the format used for logging
	handler = logging.StreamHandler()
	handler.setLevel(logging.DEBUG)
	formatter = LogFormatter(
		"(%(levelname)s)[%(asctime)s.%(msecs)03d] %(filename)s:%(lineno)d: %(message)s",
		datefmt = "%Y-%m-%d %H:%M:%S"
	)
	handler.setFormatter(formatter)
	logger.addHandler(handler)

	# Set up the device to use
	device = torch.device("cuda" if torch.cuda.is_available() else "cpu")
	logger.info(f"Using device: {device}")

	# Run the correct command
	if args.command == "train":
		assert isinstance(args.model, str)
		model = MODELS[args.model]()
		model.to(device)
		train(model, device)
	elif args.command == "evaluate":
		if isinstance(args.model, str):
			args.model = [args.model]

		for model_name in args.model:
			model = MODELS[model_name]()
			model.to(device)
			evaluate(model, device)
	else:
		parser.print_help()

	return 0


if __name__ == "__main__":
	sys.exit(main(*sys.argv[1:]))
