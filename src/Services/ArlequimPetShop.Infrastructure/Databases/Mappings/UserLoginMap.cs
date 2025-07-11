﻿using ArlequimPetShop.Domain.Users;
using FluentNHibernate.Mapping;

namespace ArlequimPetShop.Infrastructure.Databases.Mappings
{
    public class UserLoginMap : ClassMap<UserLogin>
    {
        public UserLoginMap()
        {
            Id(m => m.Id).Not.Nullable();

            Map(m => m.Email).Not.Nullable();

            Map(m => m.CreatedOn).Not.Nullable();
            Map(m => m.UpdatedOn).Not.Nullable();
            Map(m => m.DeletedOn).Nullable();

            References(m => m.User).Column("UserId").Not.Nullable().Cascade.None();
        }
    }
}