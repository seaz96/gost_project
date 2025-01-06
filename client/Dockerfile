FROM oven/bun:latest AS build

WORKDIR /app
COPY package*.json bun.lockb ./
RUN bun install
COPY . .
RUN bun run build

FROM nginx:stable
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf
ENV VITE_APP_API_URL=$API_URL
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]