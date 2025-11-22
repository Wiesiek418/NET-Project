FROM python:3.14-alpine

WORKDIR /usr/src/app

COPY requirements.txt ./
RUN pip install --no-cache-dir -r requirements.txt

COPY . .

CMD [ "python", "run_all.py --baking-furnaces 2 --conveyor-belts 1 --dough-mixers 1 --packing-lines 1" ]
