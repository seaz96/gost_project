FROM oven/bun:latest AS build

ARG API_URL
ENV VITE_APP_API_URL=$API_URL
WORKDIR /app
COPY package*.json bun.lockb ./
RUN bun install
COPY . .
RUN bun run build

FROM nginx:stable
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
