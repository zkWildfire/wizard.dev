#!/usr/bin/env python3
# Entry point for MNIST model training and evaluation.
import argparse
from typing import Callable, Dict
from mnist.models.model import IModel
from mnist.models.simple import SimpleModel
from mnist.evaluate import evaluate
from mnist.train import train
from typing import List, Dict
import sys

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

	# The model or models that were selected
	# For the "train" command, this will always be a single model. For the
	#   "evaluate" command, this will be a string or list of models.
	model: str | List[str]


def make_parser() -> argparse.ArgumentParser:
	"""
	Creates the argument parser for the MNIST training and evaluation.
	@returns The argument parser for the commands.
	"""
	parser = argparse.ArgumentParser(
		description="MNIST model training and evaluation."
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

	if args.command == "train":
		assert isinstance(args.model, str)
		model = MODELS[args.model]()
		train(model)
	elif args.command == "evaluate":
		if isinstance(args.model, str):
			args.model = [args.model]

		for model_name in args.model:
			model = MODELS[model_name]()
			evaluate(model)
	else:
		parser.print_help()

	return 0


if __name__ == "__main__":
	sys.exit(main(*sys.argv[1:]))
