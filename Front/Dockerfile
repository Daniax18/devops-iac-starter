FROM node:24-bookworm-slim

WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .
RUN npm run build

# installer un serveur statique
RUN npm install -g serve

EXPOSE 4200

CMD ["serve", "-s", "dist/front-app/browser", "-l", "4200"]