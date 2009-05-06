﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using LinqToCodedom.Generator;
using System.Linq.Expressions;

namespace LinqToCodedom.Extensions
{
    public static partial class CodeTypeDeclarationExtensions
    {
        #region Ctor

        public static CodeTypeDeclaration AddCtor(this CodeTypeDeclaration @class, CodeConstructor ctor)
        {
            @class.Members_Add(ctor);

            return @class;
        }

        public static CodeConstructor AddCtor<T>(this CodeTypeDeclaration @class,
            Expression<Func<T, MemberAttributes>> paramsAndAccessLevel,
            params CodeStatement[] statements)
        {
            var ctor = Define.Ctor(paramsAndAccessLevel, statements);

            @class.Members_Add(ctor);

            return ctor;
        }

        public static CodeConstructor AddCtor(this CodeTypeDeclaration @class,
            Expression<Func<MemberAttributes>> paramsAndAccessLevel,
            params CodeStatement[] statements)
        {
            var ctor = Define.Ctor(paramsAndAccessLevel, statements);

            @class.Members_Add(ctor);

            return ctor;
        }

        #endregion

    }
}
