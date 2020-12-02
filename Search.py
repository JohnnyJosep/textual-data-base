from enum import Enum
import json


def obj_to_dict(our_obj):
    obj_dict = {
        "__class__": our_obj.__class__.__name__,
        "__module__": our_obj.__module__
    }
    obj_dict.update(our_obj.__dict__)
    return obj_dict


def dict_to_obj(our_dict):
    if "__class__" in our_dict:
        class_name = our_dict.pop("__class__")
        module_name = our_dict.pop("__module__")
        module = __import__(module_name)
        attr = getattr(module, class_name)
        return attr(**our_dict)

    return our_dict


class Statement:
    def __init__(self, operator):
        self.operator = operator

    def to_json(self):
        return json.dumps(self, default=obj_to_dict, sort_keys=True, indent=4)

    def to_sql(self):
        pass


class RelationalOperator(str, Enum):
    AND = "and"
    OR = "or"


class RelationStatement(Statement):
    def __init__(self, statements, operator=RelationalOperator.AND):
        super().__init__(operator)
        self.statements = statements

    def to_sql(self):
        if len(self.statements) > 0:
            sqls = []
            params = []

            for s in self.statements:
                sql, param = s.to_sql()
                sqls.append(sql)
                for p in param:
                    params.append(p)

            join = f" {self.operator} ".join(sqls)
            return f'({join})', params
        else:
            return self.statements[0].to_sql()


class AttributeOperator(str, Enum):
    EQUAL = "="
    NOT_EQUAL = "/="
    CONTAINS = "like"
    NOT_CONTAINS = "not like"


class AttributeStatement(Statement):
    def __init__(self, attr_name, attr_value, operator=AttributeOperator.EQUAL):
        super().__init__(operator)
        self.attr_name = attr_name
        self.attr_value = attr_value

    def to_sql(self):
        if self.operator == AttributeOperator.CONTAINS or self.operator == AttributeOperator.NOT_CONTAINS:
            value = f'%{self.attr_value}%'
        else:
            value = self.attr_value

        return f"""
            exists(
                select 1 from attributes a inner join attribute_values av on a.id = av.attr_id 
                where 
                    av.text_id = t.id and a.name = %s and av.value {self.operator} %s)""", [self.attr_name, value]


if __name__ == "__main__":

    attrSearch = AttributeStatement(attr_name="speacker", attr_value="IGLESISAS TURRIÓN")
    relSearch = RelationStatement([attrSearch, attrSearch])
    relSearch = RelationStatement([attrSearch, relSearch], RelationalOperator.OR)

    statements_json = relSearch.to_json()
    print(statements_json)

    statement = json.loads(statements_json, object_hook=dict_to_obj)
    print(statement.to_sql()[0])
