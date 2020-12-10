
class TextData:
    def __init__(self, text, meta):
        self.text = text
        self.metadata = meta

    def __str__(self):
        stg = f"text:\t\t{self.text}\n"
        stg += "meta:\n"
        stg += "\n".join([f"\t{key}:\t{self.meta[key]}" for key in self.meta])

        return stg

if __name__ == "__main__":
    td = TextData('hola', {'speacker': 'Pepe'})
    for m in td.meta:
        print(m)