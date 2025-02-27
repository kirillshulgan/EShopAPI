#!/bin/bash
set -e

# 1. Removing EShopAPI directory
echo "Removing directory /home/client/EShopAPI..."
rm -rf /home/client/EShopAPI

# 2. Git clone EShopAPI.git
echo "Clone repository EShopAPI..."
git clone https://github.com/kirillshulgan/EShopAPI.git /home/client/EShopAPI

# 3. Stoping EShopAPI container
echo "Stoping EShopAPI container..."
cd /home/client && docker compose stop eshopapi

# 4. Build and Run EShopAPI container
echo "Build and Run EShopAPI container..."
cd /home/client && docker compose up -d --build eshopapi

# 5. Result 
echo "EShopAPI container is running!"