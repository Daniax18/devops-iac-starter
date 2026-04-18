#!/usr/bin/env bash

set -euo pipefail

DEPLOY_PATH="${DEPLOY_PATH:?DEPLOY_PATH must be set}"
DEPLOY_BRANCH="${DEPLOY_BRANCH:-develop}"

echo "Deploy path: ${DEPLOY_PATH}"
echo "Deploy branch: ${DEPLOY_BRANCH}"

# Lancer le build en arrière-plan pour éviter le timeout SSH
nohup bash -c 'docker compose up -d --build --remove-orphans && docker image prune -f' > /tmp/deploy.log 2>&1 &

if [ ! -d "${DEPLOY_PATH}" ]; then
  echo "Deployment directory does not exist: ${DEPLOY_PATH}" >&2
  exit 1
fi

cd "${DEPLOY_PATH}"

if [ ! -d .git ]; then
  echo "The deployment directory is not a git repository: ${DEPLOY_PATH}" >&2
  exit 1
fi

if [ ! -f .env ]; then
  echo "Missing .env file in ${DEPLOY_PATH}. Create it from .env.example before running CD." >&2
  exit 1
fi

git fetch origin "${DEPLOY_BRANCH}"
git checkout "${DEPLOY_BRANCH}"
git pull --ff-only origin "${DEPLOY_BRANCH}"

docker compose up -d --build --remove-orphans
docker image prune -f
docker compose ps
