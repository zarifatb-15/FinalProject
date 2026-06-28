#!/bin/bash

echo "Stopping old app on port 5003..."
lsof -ti:5003 | xargs kill -9 2>/dev/null || true

echo "Starting OnlineAuctionApp WebAPI..."
dotnet run --project src/Presentation/OnlineAuctionApp.WebAPI --launch-profile http &

APP_PID=$!

echo "Waiting for API to start..."
sleep 4

echo "Opening Scalar documentation..."
open http://localhost:5003/scalar/v1

echo "API is running. Press Ctrl+C to stop."

wait $APP_PID