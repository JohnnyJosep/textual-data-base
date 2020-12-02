import psycopg2
from config import config
import gzip
import json
from Search import RelationStatement, AttributeStatement, AttributeOperator


def insert(textData):
    compress_meta = gzip.compress(bytes(json.dumps(textData.meta), 'utf-8'))
    sql_texts = 'insert into texts (text, meta_gzip) values(%s, %s) returning id;'
    sql_attributes = 'insert into attributes (name) values (%s) on conflict (name) do update set name = excluded.name returning id;'
    sql_values = 'insert into attribute_values (text_id, attr_id, value) values (%s, %s, %s) on conflict (text_id, attr_id) do update set value = excluded.value returning text_id, attr_id;'
    conn = None
    try:
        params = config()
        conn = psycopg2.connect(**params)
        cur = conn.cursor()

        cur.execute(sql_texts, (textData.text,compress_meta,))
        text_id = cur.fetchone()[0]

        for meta_key in textData.meta:
            cur.execute(sql_attributes, (meta_key,))
            attr_id = cur.fetchone()[0]

            cur.execute(sql_values, (text_id, attr_id, textData.meta[meta_key],))

        conn.commit()
        cur.close()
    except (Exception, psycopg2.DatabaseError) as error:
        print(error)
        print(textData)
    finally:
        if conn is not None:
            conn.close()

class RetrieveResult:
    def __init__(self, row):
        self.id = row[0]
        self.text = row[1]
        self.pos = row[2]
        self.meta = json.loads(gzip.decompress(row[3]).decode('utf-8'))

def retrieve(relation_statement):
    sql_where, sql_params = relation_statement.to_sql()
    sql = f'select id, text, part_of_speach, meta_gzip from texts t where {sql_where}'

    conn = None
    try:
        params = config()
        conn = psycopg2.connect(**params)
        cur = conn.cursor()

        cur.execute(sql, tuple(sql_params))
        print("The number of parts: ", cur.rowcount)
        rows = [RetrieveResult(r) for r in cur.fetchall()]
        cur.close()

        return rows
    except (Exception, psycopg2.DatabaseError) as error:
        print(error)
    finally:
        if conn is not None:
            conn.close()


if __name__ == "__main__":
    result = retrieve(RelationStatement(statements=[AttributeStatement('speacker', 'ESTEBAN BRAVO', AttributeOperator.CONTAINS)]))
    if result is not None:
        for r in result:
            print(r)
