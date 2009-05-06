using System.CodeDom;
using System.Linq.Expressions;
using LinqToCodedom.Generator;
using System;
using System.Linq;

namespace LinqToCodedom
{
    public static class CodeTypeDeclarationExtensions
    {
        internal static CodeTypeDeclaration Members_Add(this CodeTypeDeclaration classCode, CodeTypeMember member)
        {
            //if (typeof(CodeMemberProperty).IsAssignableFrom(member.GetType()))
            //{
            //    if (classCode.IsInterface)
            //    {
            //        CodeMemberProperty prop = member as CodeMemberProperty;
            //        prop.HasGet 
            //    }
            //}

            member.UserData["parent"] = classCode;

            classCode.Members.Add(member);

            return classCode;
        }

        internal static CodeTypeDeclaration Members_AddRange(this CodeTypeDeclaration classCode, CodeTypeMember[] members)
        {
            foreach (CodeTypeMember member in members)
            {
                classCode.Members_Add(member);
            }

            return classCode;
        }

        #region Methods
        
        public static CodeTypeDeclaration AddMethod(this CodeTypeDeclaration classCode, CodeMemberMethod methodBody)
        {
            classCode.Members_Add(methodBody);

            return classCode;
        }

        public static CodeMemberMethod AddMethod<T>(this CodeTypeDeclaration classCode,
            Type returnType, MemberAttributes ma,
            Expression<Func<T, string>> paramsAndName, params CodeStatement[] statements)
        {
            var meth = Define.Method(returnType, CorrectAttributes(classCode, ma), paramsAndName, statements);

            classCode.Members_Add(meth);

            return meth;
        }

        public static CodeMemberMethod AddMethod<T>(this CodeTypeDeclaration classCode,
            string returnType, MemberAttributes ma,
            Expression<Func<T, string>> paramsAndName, params CodeStatement[] statements)
        {
            var meth = Define.Method(returnType, CorrectAttributes(classCode, ma), paramsAndName, statements);

            classCode.Members_Add(meth);

            return meth;
        }

        public static CodeMemberMethod AddMethod(this CodeTypeDeclaration classCode,
            string returnType, MemberAttributes ma,
            Expression<Func<string>> paramsAndName, params CodeStatement[] statements)
        {
            var meth = Define.Method(returnType, CorrectAttributes(classCode, ma), paramsAndName, statements);

            classCode.Members_Add(meth);

            return meth;
        }

        public static CodeMemberMethod AddMethod(this CodeTypeDeclaration classCode,
            CodeTypeReference returnType, MemberAttributes ma,
            Expression<Func<string>> paramsAndName, params CodeStatement[] statements)
        {
            var meth = Define.Method(returnType, CorrectAttributes(classCode, ma), paramsAndName, statements);

            classCode.Members_Add(meth);

            return meth;
        }

        public static CodeMemberMethod AddMethod(this CodeTypeDeclaration classCode,
            Type returnType, MemberAttributes ma,
            Expression<Func<string>> paramsAndName, params CodeStatement[] statements)
        {
            var meth = Define.Method(returnType, CorrectAttributes(classCode, ma), paramsAndName, statements);

            classCode.Members_Add(meth);

            return meth;
        }

        public static CodeMemberMethod AddMethod<T>(this CodeTypeDeclaration classCode,
            MemberAttributes ma,
            Expression<Func<T, string>> paramsAndName, params CodeStatement[] statements)
        {
            var meth = Define.Method(CorrectAttributes(classCode, ma), paramsAndName, statements);

            classCode.Members_Add(meth);

            return meth;
        }

        public static CodeMemberMethod AddMethod(this CodeTypeDeclaration classCode,
            MemberAttributes ma, Expression<Func<string>> paramsAndName, 
            params CodeStatement[] statements)
        {
            var meth = Define.Method(CorrectAttributes(classCode, ma), paramsAndName, statements);

            classCode.Members_Add(meth);

            return meth;
        }

        #endregion

        #region Properties

        public static CodeTypeDeclaration AddProperty(this CodeTypeDeclaration classCode, CodeMemberProperty property)
        {
            classCode.Members_Add(property);

            return classCode;
        }

        public static CodeMemberProperty AddProperty(this CodeTypeDeclaration classCode, 
            string propertyType, MemberAttributes ma, string name, string fieldName)
        {
            var prop = Define.Property(propertyType, CorrectAttributes(classCode, ma), name, fieldName);

            classCode.Members_Add(prop);

            return prop;
        }

        public static CodeMemberProperty AddProperty(this CodeTypeDeclaration classCode,
            Type propertyType, MemberAttributes ma, string name, string fieldName)
        {
            var prop = Define.Property(propertyType, CorrectAttributes(classCode, ma), name, fieldName);

            classCode.Members_Add(prop);

            return prop;
        }

        public static CodeMemberProperty AddProperty(this CodeTypeDeclaration classCode,
            string propertyType, MemberAttributes ma, string name)
        {
            var prop = Define.Property(propertyType, CorrectAttributes(classCode, ma), name);

            classCode.Members_Add(prop);

            return prop;
        }

        public static CodeMemberProperty AddProperty(this CodeTypeDeclaration classCode,
            Type propertyType, MemberAttributes ma, string name)
        {
            var prop = Define.Property(propertyType, CorrectAttributes(classCode, ma), name);

            classCode.Members_Add(prop);

            return prop;
        }

        public static CodeMemberProperty AddProperty(this CodeTypeDeclaration classCode, 
            Type propertyType, MemberAttributes ma, string name,
                CodeStatement[] getStatements, params CodeStatement[] setStatements)
        {
            var prop = Define.Property(propertyType, CorrectAttributes(classCode, ma), name, getStatements, setStatements);

            classCode.Members_Add(prop);

            return prop;
        }

        public static CodeMemberProperty AddProperty(this CodeTypeDeclaration classCode,
            string propertyType, MemberAttributes ma, string name,
                CodeStatement[] getStatements, params CodeStatement[] setStatements)
        {
            var prop = Define.Property(propertyType, CorrectAttributes(classCode, ma), name, getStatements, setStatements);

            classCode.Members_Add(prop);

            return prop;
        }

        public static CodeMemberProperty AddGetProperty(this CodeTypeDeclaration classCode,
            Type propertyType, MemberAttributes ma, string name,
            params CodeStatement[] statements)
        {
            var prop = Define.GetProperty(propertyType, CorrectAttributes(classCode, ma), name, statements);

            classCode.Members_Add(prop);

            return prop;
        }

        public static CodeMemberProperty AddGetProperty(this CodeTypeDeclaration classCode,
           Type propertyType, MemberAttributes ma, string name, string fieldName)
        {
            var prop = Define.GetProperty(propertyType, CorrectAttributes(classCode, ma), name, fieldName);

            classCode.Members_Add(prop);

            return prop;
        }

        public static CodeMemberProperty AddGetProperty(this CodeTypeDeclaration classCode,
           string propertyType, MemberAttributes ma, string name)
        {
            var prop = Define.GetProperty(propertyType, CorrectAttributes(classCode, ma), name);

            classCode.Members_Add(prop);

            return prop;
        }

        public static CodeMemberProperty AddGetProperty(this CodeTypeDeclaration classCode,
           string propertyType, MemberAttributes ma, string name, string fieldName)
        {
            var prop = Define.GetProperty(propertyType, CorrectAttributes(classCode, ma), name, fieldName);

            classCode.Members_Add(prop);

            return prop;
        }

        public static CodeMemberProperty Property(this CodeTypeDeclaration classCode,
            Type propertyType, MemberAttributes ma, string name,
            CodeStatement[] getStatements, params CodeStatement[] setStatements)
        {
            var prop = Define.Property(propertyType, CorrectAttributes(classCode, ma), name, getStatements, setStatements);

            classCode.Members_Add(prop);

            return prop;
        }

        #endregion

        #region Fields

        public static CodeTypeDeclaration AddFields(this CodeTypeDeclaration classCode, params CodeMemberField[] field)
        {
            classCode.Members_AddRange(field);

            return classCode;
        }

        public static CodeTypeDeclaration AddField(this CodeTypeDeclaration classCode,
            Type fieldType, MemberAttributes ma, string name)
        {
            classCode.Members_Add(Define.Field(fieldType, CorrectAttributes(classCode, ma), name));

            return classCode;
        }

        #endregion

        #region Event
         
        public static CodeTypeDeclaration AddEvents(this CodeTypeDeclaration classCode, params CodeMemberEvent[] @event)
        {
            classCode.Members_AddRange(@event);

            return classCode;
        }

        public static CodeMemberEvent AddEvent(this CodeTypeDeclaration classCode,
            string delegateType, MemberAttributes ma, string name)
        {
            var ev = Define.Event(delegateType, CorrectAttributes(classCode, ma), name);

            classCode.Members_Add(ev);

            return ev;
        }

        public static CodeMemberEvent AddEvent(this CodeTypeDeclaration classCode,
            Type delegateType, MemberAttributes ma, string name)
        {
            var ev = Define.Event(delegateType, CorrectAttributes(classCode, ma), name);

            classCode.Members_Add(ev);

            return ev;
        }

        #endregion

        #region Delegate

        public static CodeTypeDeclaration AddDelegates(this CodeTypeDeclaration classCode, params CodeTypeDelegate[] @delegate)
        {
            classCode.Members_AddRange(@delegate);

            return classCode;
        }

        public static CodeTypeDeclaration AddDelegate<T>(this CodeTypeDeclaration classCode,
            Type returnType, MemberAttributes ma, Expression<Func<T, string>> paramsAndName)
        {
            classCode.Members_Add(Define.Delegate(returnType, CorrectAttributes(classCode, ma), paramsAndName));

            return classCode;
        }

        public static CodeTypeDeclaration AddDelegate<T>(this CodeTypeDeclaration classCode,
            string returnType, MemberAttributes ma, Expression<Func<T, string>> paramsAndName)
        {
            classCode.Members_Add(Define.Delegate(returnType, CorrectAttributes(classCode, ma), paramsAndName));

            return classCode;
        }

        #endregion

        #region Ctor

        public static CodeTypeDeclaration AddCtor(this CodeTypeDeclaration @class, CodeConstructor ctor)
        {
            @class.Members_Add(ctor);

            return @class;
        }

        public static CodeTypeDeclaration AddCtor<T>(this CodeTypeDeclaration @class,
            Expression<Func<T, MemberAttributes>> paramsAndAccessLevel,
            params CodeStatement[] statements)
        {
            @class.Members_Add(Define.Ctor(paramsAndAccessLevel, statements));

            return @class;
        }

        #endregion

        public static CodeTypeDeclaration AddClass(this CodeTypeDeclaration classCode, CodeTypeDeclaration codeType)
        {
            classCode.Members_Add(codeType);

            return classCode;
        }

        public static CodeTypeDeclaration Inherits(this CodeTypeDeclaration classCode, string type)
        {
            classCode.BaseTypes.Add(type);
            return classCode;
        }

        public static CodeTypeDeclaration Inherits(this CodeTypeDeclaration classCode, Type type)
        {
            classCode.BaseTypes.Add(type);
            return classCode;
        }

        public static CodeTypeDeclaration Inherits(this CodeTypeDeclaration classCode, CodeTypeReference type)
        {
            classCode.BaseTypes.Add(type);
            return classCode;
        }

        public static CodeTypeDeclaration Implements(this CodeTypeDeclaration classCode, params string[] type)
        {
            classCode.BaseTypes.AddRange(type.Select((t)=>new CodeTypeReference(t)).ToArray());
            return classCode;
        }

        public static CodeTypeDeclaration Implements(this CodeTypeDeclaration classCode, params Type[] type)
        {
            classCode.BaseTypes.AddRange(type.Select((t) => new CodeTypeReference(t)).ToArray());
            return classCode;
        }

        public static CodeTypeDeclaration Implements(this CodeTypeDeclaration classCode, params CodeTypeReference[] type)
        {
            classCode.BaseTypes.AddRange(type);
            return classCode;
        }

        private static MemberAttributes CorrectAttributes(CodeTypeDeclaration classDecl, MemberAttributes ma)
        {
            if (classDecl.IsInterface/* || (classDecl.Attributes & MemberAttributes.Abstract) == MemberAttributes.Abstract*/)
            {
                ma |= MemberAttributes.Abstract;
                ma &= ~MemberAttributes.Public;
            }
            return ma;
        }
    }
}
