#!/usr/bin/env sh
# Usage: ./publish.sh
# Helper script used to deploy the documentation to GitHub pages.
set -e

# Get the path to the folder containing this script
SCRIPT_DIR=$(cd "$(dirname "$0")" && pwd)
readonly SCRIPT_DIR

# Get the repository root directory
REPO_ROOT=$(realpath "${SCRIPT_DIR}/../..")
readonly REPO_ROOT

# Check if force publishing was requested
if [ -z "${FORCE_PUBLISH}" ]; then
	readonly FORCE_FLAG=""
else
	readonly FORCE_FLAG="--force"
fi

# Set up SSH keys
# `PUBLISH_SSH_KEY` will be an environment variable set by docker-compose.yml
eval "$(ssh-agent -s)"
echo "${PUBLISH_SSH_KEY}" | ssh-add -

# Configure git
git config --global user.name "GitHub Actions"
git config --global user.email "gh-actions@wizard.dev"

# This is necessary when running the script via GitHub Actions
git config --global --add safe.directory "${REPO_ROOT}"

# Add the GitHub remote as a trusted SSH host
mkdir -p ~/.ssh
ssh-keyscan -t rsa github.com >> ~/.ssh/known_hosts

# Pull the latest changes to make sure that the gh-pages branch is up to date
cd "${REPO_ROOT}"
git pull

# Publish the documentation
mkdocs gh-deploy ${FORCE_FLAG}
