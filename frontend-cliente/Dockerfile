
FROM node:20-alpine AS builder

WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .
RUN npm run build

# Etapa 2: Imagen final, solo archivos estáticos servidos
FROM nginx:stable-alpine

# Configurar nginx para servir el front
COPY nginx.conf /etc/nginx/nginx.conf

# Copiar archivos estáticos generados por Vite
COPY --from=builder /app/dist /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]