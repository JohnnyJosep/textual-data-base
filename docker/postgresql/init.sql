
create table texts 
(
    id              serial primary key,
    text            text not null,
    part_of_speach  text null
);

create table attributes 
(
    id              serial primary key,
    name            varchar(255) not null
);

create table attribute_values
(
	text_id         int             constraint attribute_values_texts_id_fk references texts,
	attr_id         int             constraint attribute_values_attributes_id_fk references attributes,
	value           text not null,
	constraint attribute_values_pk primary key (text_id, attr_id)
);