
FROM node:12.18 as build-stage

WORKDIR /app

COPY package*.json /app/

RUN npm install

COPY ./ /app/

ARG configuration=production

RUN npm run build -- --output-path=./dist/out --configuration $configuration
# Stage 1, based on Nginx, to have only the compiled app, ready for production with Nginx
FROM nginx:1.15

## Remove default nginx website
RUN rm -rf /usr/share/nginx/html/*

## Copy the ui artifacts to www public folder
COPY --from=build-stage /app/dist/out/ /var/www/html

## Copy nginx configuration
COPY ./nginx.conf /etc/nginx/nginx.conf

## Copy nginx locations configuration
COPY ./locations/prod/frontend.conf /etc/nginx/locations/frontend.conf