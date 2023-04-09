# EasyExerise (mobile)
EasyExerise is a game like ringfit (clone) using unity (This is Computer Science Project).

# Getting started
# Game pc
 1. clone [Game-project](https://github.com/Sitthiphong-14/CS-EasyExcerice-Game)
 2. open it with [unity 2021](https://unity.com/releases/2021-lts)
 5. click run
 3. connect android device that using same wifi to this game

# Mobile
 1. clone [mobile-project](https://github.com/Ford-Narongrit/CS-EasyExercies-mobile)
 2. open it with [unity 2021](https://unity.com/releases/2021-lts)
 3. download [unity remote 5](https://play.google.com/store/apps/details?id=com.unity3d.mobileremote&hl=en_US) to test in android device.
 4. connect android that open unity remote 5 to pc
 5. click run

# Backend
this backend to store data, you can get backend project [EasyExerise-Backend](https://github.com/Ford-Narongrit/EasyExercise-backend)
backend project this using Laravel and mysql to developed.

 1. install composer Dependencies
``` bash
docker run --rm \
    -u "$(id -u):$(id -g)" \
    -v $(pwd):/var/www/html \
    -w /var/www/html \
    laravelsail/php81-composer:latest \
    composer install --ignore-platform-reqs
```
2. config .env
```bash
cp .env.example .env
```
3. alias sail
```bash
alias sail='[ -f sail ] && bash sail || bash vendor/bin/sail' 
```
4. Build container with sail
```bash
sail up -d
```

5. SetUP .ENV  
```bash
sail artisan jwt:secret
sail artisan key:generate
sail artisan storage:link
```
6. Migration
```bash
sail artisan migrate
```

# release version
[mobile-1.0.0](https://github.com/Ford-Narongrit/CS-EasyExercies-mobile/releases/tag/1.0.0)

[GamePC-1.0.0](https://github.com/Sitthiphong-14/CS-EasyExcerice-Game/releases/tag/1.0.0)
